#!/bin/bash

echo "🔍 Checking prerequisites..."
echo ""

# Color codes
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

check_command() {
    if command -v $1 &> /dev/null; then
        VERSION=$($1 --version 2>&1 | head -n1)
        echo -e "${GREEN}✓${NC} $1 installed: $VERSION"
        return 0
    else
        echo -e "${RED}✗${NC} $1 NOT installed"
        return 1
    fi
}

ERRORS=0

# Check required tools
check_command terraform || ((ERRORS++))
check_command ansible || ((ERRORS++))
check_command aws || ((ERRORS++))
check_command python3 || ((ERRORS++))

echo ""
echo "🔑 Checking AWS credentials..."
if aws sts get-caller-identity &> /dev/null; then
    ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
    REGION=$(aws configure get region)
    echo -e "${GREEN}✓${NC} AWS configured (Account: $ACCOUNT_ID, Region: $REGION)"
else
    echo -e "${RED}✗${NC} AWS credentials not configured"
    ((ERRORS++))
fi

echo ""
echo "📁 Checking project structure..."
[ -f "terraform/main.tf" ] && echo -e "${GREEN}✓${NC} Terraform files found" || echo -e "${RED}✗${NC} Terraform files missing"
[ -f "ansible/ansible.cfg" ] && echo -e "${GREEN}✓${NC} Ansible files found" || echo -e "${RED}✗${NC} Ansible files missing"

echo ""
if [ $ERRORS -eq 0 ]; then
    echo -e "${GREEN}✅ All checks passed! Ready to deploy.${NC}"
    exit 0
else
    echo -e "${RED}❌ $ERRORS error(s) found. Please fix before proceeding.${NC}"
    exit 1
fi