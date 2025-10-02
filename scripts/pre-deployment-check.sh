#!/bin/bash
# scripts/pre-deployment-check.sh
# Pre-deployment validation to catch issues before deployment

set -e

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

ERRORS=0
WARNINGS=0

echo -e "${BLUE}╔════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BLUE}║        PRE-DEPLOYMENT VALIDATION SYSTEM                    ║${NC}"
echo -e "${BLUE}╚════════════════════════════════════════════════════════════╝${NC}"

error() {
    echo -e "${RED}✗ ERROR: $1${NC}"
    ((ERRORS++))
}

warning() {
    echo -e "${YELLOW}⚠ WARNING: $1${NC}"
    ((WARNINGS++))
}

success() {
    echo -e "${GREEN}✓ $1${NC}"
}

check_section() {
    echo ""
    echo -e "${BLUE}━━━ $1 ━━━${NC}"
}

# ============================================
# 1. Check Required Tools
# ============================================
check_section "Checking Required Tools"

command -v terraform >/dev/null 2>&1 && success "Terraform installed" || error "Terraform not found"
command -v ansible >/dev/null 2>&1 && success "Ansible installed" || error "Ansible not found"
command -v aws >/dev/null 2>&1 && success "AWS CLI installed" || error "AWS CLI not found"
command -v jq >/dev/null 2>&1 && success "jq installed" || warning "jq not found (recommended)"
command -v flyway >/dev/null 2>&1 && success "Flyway installed" || warning "Flyway not found (will be installed during deployment)"

# ============================================
# 2. Check AWS Credentials
# ============================================
check_section "Checking AWS Credentials"

if aws sts get-caller-identity >/dev/null 2>&1; then
    ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
    AWS_REGION=$(aws configure get region || echo "not set")
    success "AWS credentials valid (Account: $ACCOUNT_ID, Region: $AWS_REGION)"
else
    error "AWS credentials not configured or invalid"
fi

# ============================================
# 3. Check Configuration Files
# ============================================
check_section "Checking Configuration Files"

COUNTRY=${1:-jp}
ENVIRONMENT=${2:-demo}
TFVARS_FILE="terraform/countries/${COUNTRY}/${ENVIRONMENT}.tfvars"

if [ -f "$TFVARS_FILE" ]; then
    success "Configuration file found: $TFVARS_FILE"
    
    # Validate required variables
    required_vars=("country_code" "aws_region" "vpc_cidr" "db_password" "windows_password")
    for var in "${required_vars[@]}"; do
        if grep -q "^${var}" "$TFVARS_FILE"; then
            success "  - $var defined"
        else
            error "  - $var missing in tfvars"
        fi
    done
else
    error "Configuration file not found: $TFVARS_FILE"
fi

# ============================================
# 4. Check Application Files
# ============================================
check_section "Checking Application Files"

# UI Application
if [ -d "applications/ui" ]; then
    success "UI application directory exists"
    
    if [ -f "applications/ui/composer.json" ]; then
        success "  - Laravel composer.json found"
    else
        warning "  - composer.json not found (might not be a Laravel app)"
    fi
    
    if [ -f "applications/ui/.env.example" ]; then
        success "  - .env.example found"
    else
        warning "  - .env.example not found"
    fi
else
    error "UI application directory not found: applications/ui"
fi

# API Application
if [ -d "applications/api" ]; then
    success "API application directory exists"
    
    # Check for .NET project file
    if ls applications/api/*.csproj 1> /dev/null 2>&1; then
        success "  - .NET project file found"
    else
        warning "  - .csproj file not found"
    fi
else
    error "API application directory not found: applications/api"
fi

# Database Migrations
if [ -d "applications/database" ]; then
    success "Database migration directory exists"
    
    migration_folders=("1. Table & Trigger" "2. TableType" "3. View" "4. Function" "5. StoredProcedure" "6. Data Initialization")
    for folder in "${migration_folders[@]}"; do
        if [ -d "applications/database/${folder}" ]; then
            sql_count=$(find "applications/database/${folder}" -name "*.sql" 2>/dev/null | wc -l)
            if [ $sql_count -gt 0 ]; then
                success "  - ${folder}: $sql_count SQL files"
            else
                warning "  - ${folder}: no SQL files found"
            fi
        else
            warning "  - ${folder}: directory not found"
        fi
    done
else
    error "Database migration directory not found: applications/database"
fi

# ============================================
# 5. Check Ansible Configuration
# ============================================
check_section "Checking Ansible Configuration"

if [ -f "ansible/ansible.cfg" ]; then
    success "Ansible configuration found"
else
    warning "ansible.cfg not found"
fi

if [ -f "ansible/vault-password.txt" ]; then
    success "Vault password file found"
else
    warning "Vault password file not found (encrypted vars may not work)"
fi

# Check playbooks
playbooks=("configure-ui.yml" "setup-ssl.yml")
for playbook in "${playbooks[@]}"; do
    if [ -f "ansible/playbooks/$playbook" ]; then
        success "  - Playbook: $playbook"
    else
        warning "  - Playbook not found: $playbook"
    fi
done

# ============================================
# 6. Check Terraform Configuration
# ============================================
check_section "Checking Terraform Configuration"

cd terraform

if terraform init -backend=false >/dev/null 2>&1; then
    success "Terraform initialization successful"
else
    error "Terraform initialization failed"
fi

if terraform validate >/dev/null 2>&1; then
    success "Terraform validation passed"
else
    error "Terraform validation failed"
fi

cd ..

# ============================================
# 7. Check DNS Configuration
# ============================================
check_section "Checking DNS Configuration"

DOMAIN_BASE=$(grep domain_base "$TFVARS_FILE" | cut -d'=' -f2 | tr -d ' "')
UI_SUBDOMAIN=$(grep ui_subdomain "$TFVARS_FILE" | cut -d'=' -f2 | tr -d ' "')
API_SUBDOMAIN=$(grep api_subdomain "$TFVARS_FILE" | cut -d'=' -f2 | tr -d ' "')

if [ -n "$DOMAIN_BASE" ]; then
    success "Domain configured: $DOMAIN_BASE"
    
    # Check if hosted zone exists in AWS
    ZONE_ID=$(aws route53 list-hosted-zones-by-name --dns-name "$DOMAIN_BASE" --query "HostedZones[0].Id" --output text 2>/dev/null || echo "")
    
    if [ -n "$ZONE_ID" ] && [ "$ZONE_ID" != "None" ]; then
        success "  - Route53 hosted zone found: $ZONE_ID"
    else
        warning "  - Route53 hosted zone not found for $DOMAIN_BASE"
    fi
else
    error "Domain base not configured in tfvars"
fi

# ============================================
# 8. Check Network Connectivity
# ============================================
check_section "Checking Network Connectivity"

# Check if we can reach AWS
if ping -c 1 aws.amazon.com >/dev/null 2>&1; then
    success "Internet connectivity OK"
else
    warning "Cannot reach aws.amazon.com (might be network issue)"
fi

# ============================================
# 9. Disk Space Check
# ============================================
check_section "Checking Disk Space"

AVAILABLE=$(df -h . | awk 'NR==2 {print $4}')
success "Available disk space: $AVAILABLE"

# ============================================
# 10. Security Checks
# ============================================
check_section "Security Checks"

# Check for sensitive files in git
if [ -f ".gitignore" ]; then
    sensitive_patterns=("*.pem" "*.key" "*.tfstate" "vault-password.txt" "*.tfvars")
    for pattern in "${sensitive_patterns[@]}"; do
        if grep -q "$pattern" .gitignore; then
            success "  - $pattern in .gitignore"
        else
            warning "  - $pattern not in .gitignore (security risk)"
        fi
    done
else
    warning ".gitignore not found"
fi

# Check for hardcoded secrets
if grep -r "password.*=" terraform/ --include="*.tf" 2>/dev/null | grep -v "variable\|description" | grep -q .; then
    warning "Potential hardcoded passwords found in Terraform files"
else
    success "No obvious hardcoded passwords in Terraform"
fi

# ============================================
# Summary
# ============================================
echo ""
echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"
echo -e "${BLUE}  VALIDATION SUMMARY${NC}"
echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"

if [ $ERRORS -eq 0 ] && [ $WARNINGS -eq 0 ]; then
    echo -e "${GREEN}✓ All checks passed! Ready to deploy.${NC}"
    exit 0
elif [ $ERRORS -eq 0 ]; then
    echo -e "${YELLOW}⚠ ${WARNINGS} warning(s) found${NC}"
    echo -e "${YELLOW}  Deployment can proceed, but review warnings above${NC}"
    exit 0
else
    echo -e "${RED}✗ ${ERRORS} error(s) and ${WARNINGS} warning(s) found${NC}"
    echo -e "${RED}  Please fix errors before deployment${NC}"
    exit 1
fi