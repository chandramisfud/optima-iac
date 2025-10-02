#!/bin/bash
# applications/database/run-flyway-migration.sh
# Automated Flyway Migration Script

set -e

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"
echo -e "${BLUE}  OPTIMA DATABASE MIGRATION - Flyway${NC}"
echo -e "${BLUE}═══════════════════════════════════════════════════════════${NC}"

# Configuration
COUNTRY=${1:-jp}
ENVIRONMENT=${2:-demo}
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
FLYWAY_CONF="${SCRIPT_DIR}/flyway.conf"

# Check if flyway is installed
if ! command -v flyway &> /dev/null; then
    echo -e "${YELLOW}⚠ Flyway not found. Installing...${NC}"
    
    # Detect OS
    if [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS
        brew install flyway
    elif [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux
        wget -qO- https://download.red-gate.com/maven/release/com/redgate/flyway/flyway-commandline/10.8.1/flyway-commandline-10.8.1-linux-x64.tar.gz | tar -xvz
        sudo ln -s $(pwd)/flyway-10.8.1/flyway /usr/local/bin/flyway
    else
        echo -e "${RED}✗ Unsupported OS. Please install Flyway manually.${NC}"
        exit 1
    fi
fi

echo -e "${GREEN}✓ Flyway found${NC}"

# Load database configuration from Terraform outputs
echo -e "${YELLOW}→ Loading database configuration...${NC}"

if [ -f "../../terraform/terraform.tfstate" ]; then
    DB_ENDPOINT=$(cd ../../terraform && terraform output -raw rds_endpoint 2>/dev/null || echo "")
    
    if [ -z "$DB_ENDPOINT" ]; then
        echo -e "${RED}✗ Could not get RDS endpoint from Terraform${NC}"
        exit 1
    fi
    
    DB_HOST=$(echo $DB_ENDPOINT | cut -d: -f1)
    DB_PORT=$(echo $DB_ENDPOINT | cut -d: -f2)
    
    echo -e "${GREEN}✓ Database: ${DB_HOST}:${DB_PORT}${NC}"
else
    echo -e "${RED}✗ Terraform state not found${NC}"
    exit 1
fi

# Get credentials from tfvars
TFVARS_FILE="../../terraform/countries/${COUNTRY}/${ENVIRONMENT}.tfvars"

if [ ! -f "$TFVARS_FILE" ]; then
    echo -e "${RED}✗ Configuration file not found: $TFVARS_FILE${NC}"
    exit 1
fi

DB_USERNAME=$(grep db_username $TFVARS_FILE | cut -d'=' -f2 | tr -d ' "')
DB_PASSWORD=$(grep db_password $TFVARS_FILE | cut -d'=' -f2 | tr -d ' "')
DB_NAME="optima_${COUNTRY}"

# Create/Update flyway.conf
echo -e "${YELLOW}→ Generating Flyway configuration...${NC}"

cat > "$FLYWAY_CONF" << EOF
# Flyway Configuration for Optima ${COUNTRY^^} - ${ENVIRONMENT^^}
# Generated: $(date)

# JDBC connection string
flyway.url=jdbc:sqlserver://${DB_HOST}:${DB_PORT};databaseName=${DB_NAME};encrypt=true;trustServerCertificate=true

# Database credentials
flyway.user=${DB_USERNAME}
flyway.password=${DB_PASSWORD}

# Migration locations (in order)
flyway.locations=filesystem:./1. Table & Trigger,filesystem:./2. TableType,filesystem:./3. View,filesystem:./4. Function,filesystem:./5. StoredProcedure,filesystem:./6. Data Initialization

# Baseline settings
flyway.baseline-on-migrate=true
flyway.baseline-version=0
flyway.baseline-description=Initial baseline

# Schema settings
flyway.schemas=dbo
flyway.defaultSchema=dbo

# Validation
flyway.validate-on-migrate=true
flyway.clean-disabled=true

# Encoding
flyway.encoding=UTF-8

# Placeholders
flyway.placeholders.country=${COUNTRY^^}
flyway.placeholders.environment=${ENVIRONMENT^^}
flyway.placeholders.date=$(date +%Y%m%d)

# Output
flyway.outputType=json
EOF

echo -e "${GREEN}✓ Configuration generated${NC}"

# Check database connectivity
echo -e "${YELLOW}→ Testing database connectivity...${NC}"

flyway info -configFiles="$FLYWAY_CONF" > /dev/null 2>&1
if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Database connection successful${NC}"
else
    echo -e "${RED}✗ Cannot connect to database${NC}"
    echo -e "${YELLOW}  This might be normal if the database is still initializing${NC}"
    echo -e "${YELLOW}  Waiting 30 seconds and retrying...${NC}"
    sleep 30
    
    flyway info -configFiles="$FLYWAY_CONF" > /dev/null 2>&1
    if [ $? -ne 0 ]; then
        echo -e "${RED}✗ Database connection failed${NC}"
        exit 1
    fi
    echo -e "${GREEN}✓ Database connection successful (retry)${NC}"
fi

# Show current migration status
echo -e "${YELLOW}→ Current migration status:${NC}"
flyway info -configFiles="$FLYWAY_CONF"

# Run migrations
echo -e "${YELLOW}→ Running database migrations...${NC}"
flyway migrate -configFiles="$FLYWAY_CONF"

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Migration completed successfully${NC}"
else
    echo -e "${RED}✗ Migration failed${NC}"
    exit 1
fi

# Show final status
echo -e "${YELLOW}→ Final migration status:${NC}"
flyway info -configFiles="$FLYWAY_CONF"

# Validate migrations
echo -e "${YELLOW}→ Validating migrations...${NC}"
flyway validate -configFiles="$FLYWAY_CONF"

if [ $? -eq 0 ]; then
    echo -e "${GREEN}✓ Validation passed${NC}"
else
    echo -e "${YELLOW}⚠ Validation warnings detected${NC}"
fi

echo ""
echo -e "${GREEN}═══════════════════════════════════════════════════════════${NC}"
echo -e "${GREEN}  DATABASE MIGRATION COMPLETED${NC}"
echo -e "${GREEN}═══════════════════════════════════════════════════════════${NC}"
echo -e "  Database: ${DB_NAME}"
echo -e "  Host: ${DB_HOST}"
echo ""