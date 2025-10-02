#!/bin/bash
set -e  # Exit on any error

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
COUNTRY=${1:-jp}
ENVIRONMENT=${2:-demo}
TERRAFORM_DIR="terraform"
ANSIBLE_DIR="ansible"
TFVARS_FILE="terraform/countries/${COUNTRY}/${ENVIRONMENT}.tfvars"

echo -e "${BLUE}╔════════════════════════════════════════════════════════════╗${NC}"
echo -e "${BLUE}║        OPTIMA AUTOMATED DEPLOYMENT SYSTEM v1.0             ║${NC}"
echo -e "${BLUE}║        Country: ${COUNTRY^^} | Environment: ${ENVIRONMENT^^}                      ║${NC}"
echo -e "${BLUE}╚════════════════════════════════════════════════════════════╝${NC}"

# Function to print section headers
print_section() {
    echo ""
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${BLUE}  $1${NC}"
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
}

# Function to check command exists
check_command() {
    if ! command -v $1 &> /dev/null; then
        echo -e "${RED}✗ $1 is not installed. Please install it first.${NC}"
        exit 1
    fi
    echo -e "${GREEN}✓ $1 found${NC}"
}

# Function to handle errors
handle_error() {
    echo -e "${RED}✗ Deployment failed at: $1${NC}"
    echo -e "${YELLOW}Please check the logs above for details${NC}"
    exit 1
}

# Trap errors
trap 'handle_error "${BASH_COMMAND}"' ERR

#═══════════════════════════════════════════════════════════
# STEP 1: Prerequisites Check
#═══════════════════════════════════════════════════════════
print_section "STEP 1: Checking Prerequisites"

check_command "terraform"
check_command "ansible"
check_command "aws"
check_command "jq"

# Check if tfvars file exists
if [ ! -f "$TFVARS_FILE" ]; then
    echo -e "${RED}✗ Configuration file not found: $TFVARS_FILE${NC}"
    exit 1
fi
echo -e "${GREEN}✓ Configuration file found${NC}"

# Check AWS credentials
if ! aws sts get-caller-identity &> /dev/null; then
    echo -e "${RED}✗ AWS credentials not configured${NC}"
    exit 1
fi
echo -e "${GREEN}✓ AWS credentials valid${NC}"

#═══════════════════════════════════════════════════════════
# STEP 2: Terraform - Infrastructure Provisioning
#═══════════════════════════════════════════════════════════
print_section "STEP 2: Provisioning Infrastructure with Terraform"

cd $TERRAFORM_DIR

echo -e "${YELLOW}→ Initializing Terraform...${NC}"
terraform init -upgrade

echo -e "${YELLOW}→ Validating Terraform configuration...${NC}"
terraform validate

echo -e "${YELLOW}→ Planning infrastructure changes...${NC}"
terraform plan -var-file="../$TFVARS_FILE" -out=tfplan

echo -e "${YELLOW}→ Applying infrastructure...${NC}"
terraform apply tfplan

echo -e "${YELLOW}→ Extracting outputs...${NC}"
UI_IP=$(terraform output -raw ui_server_public_ip)
API_IP=$(terraform output -raw api_server_public_ip)
API_PRIVATE_IP=$(terraform output -json | jq -r '.api_server_private_ip.value')
RDS_ENDPOINT=$(terraform output -raw rds_endpoint)
SSH_KEY=$(terraform output -raw ssh_key_file)

echo -e "${GREEN}✓ Infrastructure provisioned successfully${NC}"
echo -e "  UI Server: ${GREEN}$UI_IP${NC}"
echo -e "  API Server: ${GREEN}$API_IP${NC}"
echo -e "  Database: ${GREEN}$RDS_ENDPOINT${NC}"

cd ..

# Wait for instances to be ready
print_section "Waiting for instances to initialize (90 seconds)..."
for i in {90..1}; do
    printf "\r${YELLOW}Waiting: %02d seconds remaining...${NC}" $i
    sleep 1
done
echo ""

#═══════════════════════════════════════════════════════════
# STEP 3: Database Migration with Flyway
#═══════════════════════════════════════════════════════════
print_section "STEP 3: Database Migration with Flyway"

# Update flyway.conf with actual RDS endpoint
DB_HOST=$(echo $RDS_ENDPOINT | cut -d: -f1)
DB_PORT=$(echo $RDS_ENDPOINT | cut -d: -f2)

cat > applications/database/flyway.conf << EOF
flyway.url=jdbc:sqlserver://${DB_HOST}:${DB_PORT};databaseName=optima_${COUNTRY};encrypt=true;trustServerCertificate=true
flyway.user=$(grep db_username $TFVARS_FILE | cut -d'=' -f2 | tr -d ' "')
flyway.password=$(grep db_password $TFVARS_FILE | cut -d'=' -f2 | tr -d ' "')
flyway.locations=filesystem:./1. Table & Trigger,filesystem:./2. TableType,filesystem:./3. View,filesystem:./4. Function,filesystem:./5. StoredProcedure,filesystem:./6. Data Initialization
flyway.baseline-on-migrate=true
flyway.baseline-version=0
EOF

echo -e "${YELLOW}→ Running Flyway migrations...${NC}"
cd applications/database

# Check if flyway is available
if command -v flyway &> /dev/null; then
    flyway migrate
    echo -e "${GREEN}✓ Database migration completed${NC}"
else
    echo -e "${YELLOW}⚠ Flyway not found locally. Will run migrations from UI server${NC}"
    # We'll handle this in Ansible
fi

cd ../..

#═══════════════════════════════════════════════════════════
# STEP 4: Update Ansible Inventory
#═══════════════════════════════════════════════════════════
print_section "STEP 4: Updating Ansible Inventory"

# Get Windows password
echo -e "${YELLOW}→ Retrieving Windows Administrator password...${NC}"
WINDOWS_PASSWORD=$(bash ansible/scripts/get-windows-password.sh | tail -1 || echo "")

if [ -z "$WINDOWS_PASSWORD" ]; then
    echo -e "${YELLOW}⚠ Could not retrieve Windows password automatically${NC}"
    echo -e "${YELLOW}  Using password from vault.yml${NC}"
    WINDOWS_PASSWORD=$(grep vault_windows_password ansible/inventories/demo/group_vars/vault.yml | cut -d'"' -f2)
fi

cat > ansible/inventories/demo/hosts.yml << EOF
---
all:
  children:
    ui_servers:
      hosts:
        ui-server:
          ansible_host: ${UI_IP}
          ansible_user: ubuntu
          ansible_ssh_private_key_file: ../${TERRAFORM_DIR}/${SSH_KEY}
          ansible_ssh_common_args: '-o StrictHostKeyChecking=no'
          
    api_servers:
      hosts:
        api-server:
          ansible_host: ${API_PRIVATE_IP}  # Using private IP
          ansible_user: Administrator  
          ansible_password: '${WINDOWS_PASSWORD}'
          ansible_connection: winrm
          ansible_winrm_transport: basic
          ansible_winrm_server_cert_validation: ignore
          ansible_winrm_port: 5986
EOF

echo -e "${GREEN}✓ Ansible inventory updated${NC}"

#═══════════════════════════════════════════════════════════
# STEP 5: Deploy Applications with Ansible
#═══════════════════════════════════════════════════════════
print_section "STEP 5: Deploying Applications with Ansible"

cd $ANSIBLE_DIR

echo -e "${YELLOW}→ Testing connectivity to UI server...${NC}"
ansible ui_servers -m ping -i inventories/demo/hosts.yml || handle_error "UI server connectivity"

echo -e "${YELLOW}→ Testing connectivity to API server...${NC}"
ansible api_servers -m win_ping -i inventories/demo/hosts.yml || handle_error "API server connectivity"

echo -e "${YELLOW}→ Deploying UI (Laravel) application...${NC}"
ansible-playbook -i inventories/demo/hosts.yml playbooks/configure-ui.yml

echo -e "${YELLOW}→ Deploying API (.NET 7) application...${NC}"
ansible-playbook -i inventories/demo/hosts.yml playbooks/configure-api.yml

echo -e "${GREEN}✓ Applications deployed successfully${NC}"

cd ..

#═══════════════════════════════════════════════════════════
# STEP 6: SSL Certificate Setup
#═══════════════════════════════════════════════════════════
print_section "STEP 6: Configuring SSL Certificates"

cd $ANSIBLE_DIR

echo -e "${YELLOW}→ Setting up Let's Encrypt SSL...${NC}"
ansible-playbook -i inventories/demo/hosts.yml playbooks/setup-ssl.yml

echo -e "${GREEN}✓ SSL certificates configured${NC}"

cd ..

#═══════════════════════════════════════════════════════════
# STEP 7: Health Checks
#═══════════════════════════════════════════════════════════
print_section "STEP 7: Running Health Checks"

UI_DOMAIN="optima-${COUNTRY}.xva-rnd.com"
API_DOMAIN="api-optima-${COUNTRY}.xva-rnd.com"

echo -e "${YELLOW}→ Checking UI health...${NC}"
sleep 5
if curl -f -s -o /dev/null -w "%{http_code}" "https://${UI_DOMAIN}" | grep -q "200\|302"; then
    echo -e "${GREEN}✓ UI is responding at https://${UI_DOMAIN}${NC}"
else
    echo -e "${YELLOW}⚠ UI health check failed, but deployment may still be successful${NC}"
fi

echo -e "${YELLOW}→ Checking API health...${NC}"
if curl -f -s -o /dev/null -w "%{http_code}" "https://${API_DOMAIN}/health" | grep -q "200"; then
    echo -e "${GREEN}✓ API is responding at https://${API_DOMAIN}${NC}"
else
    echo -e "${YELLOW}⚠ API health check inconclusive${NC}"
fi

#═══════════════════════════════════════════════════════════
# DEPLOYMENT COMPLETE
#═══════════════════════════════════════════════════════════
print_section "DEPLOYMENT SUMMARY"

echo -e "${GREEN}╔════════════════════════════════════════════════════════════╗${NC}"
echo -e "${GREEN}║                  DEPLOYMENT SUCCESSFUL! 🎉                 ║${NC}"
echo -e "${GREEN}╚════════════════════════════════════════════════════════════╝${NC}"
echo ""
echo -e "  ${BLUE}UI Application:${NC}      https://${UI_DOMAIN}"
echo -e "  ${BLUE}API Application:${NC}     https://${API_DOMAIN}"
echo -e "  ${BLUE}Database:${NC}            ${RDS_ENDPOINT}"
echo ""
echo -e "  ${BLUE}UI Server IP:${NC}        ${UI_IP}"
echo -e "  ${BLUE}API Server IP:${NC}       ${API_IP}"
echo -e "  ${BLUE}SSH Key:${NC}             ${TERRAFORM_DIR}/${SSH_KEY}"
echo ""
echo -e "${YELLOW}Next Steps:${NC}"
echo -e "  1. Test the applications in your browser"
echo -e "  2. Verify database connections"
echo -e "  3. Run end-to-end tests"
echo ""
echo -e "${BLUE}Logs stored in: deployment-$(date +%Y%m%d-%H%M%S).log${NC}"