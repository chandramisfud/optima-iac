#!/bin/bash

# Script to restructure database directory to Flyway-compatible format (Option 3)
# This script will create a new 'sql' directory with proper Flyway structure

set -e

echo "=== Flyway Directory Restructure Script ==="
echo ""

# Define base directory
BASE_DIR="applications/database"
NEW_SQL_DIR="${BASE_DIR}/sql"

# Check if base directory exists
if [ ! -d "$BASE_DIR" ]; then
    echo "Error: Directory '$BASE_DIR' not found!"
    exit 1
fi

# Create backup
BACKUP_DIR="${BASE_DIR}_backup_$(date +%Y%m%d_%H%M%S)"
echo "Creating backup at: $BACKUP_DIR"
cp -r "$BASE_DIR" "$BACKUP_DIR"
echo "✓ Backup created successfully"
echo ""

# Create new sql directory structure
echo "Creating new directory structure..."
mkdir -p "$NEW_SQL_DIR"

# Define directory mappings (old -> new)
declare -A DIR_MAPPING=(
    ["1. Table & Trigger"]="01_tables_and_triggers"
    ["2. TableType"]="02_table_types"
    ["3. View"]="03_views"
    ["4. Function"]="04_functions"
    ["5. StoredProcedure"]="05_stored_procedures"
    ["6. Data Initialization"]="06_data_initialization"
)

# Define file renaming (old file -> new file)
declare -A FILE_MAPPING=(
    ["1. Table & Trigger/Optima-Table.sql"]="01_tables_and_triggers/V1__create_tables_and_triggers.sql"
    ["2. TableType/Optima-TableType.sql"]="02_table_types/V2__create_table_types.sql"
    ["3. View/Optima-View.sql"]="03_views/V3__create_views.sql"
    ["4. Function/Optima-Function.sql"]="04_functions/V4__create_functions.sql"
    ["5. StoredProcedure/Optima-StoredProcedure.sql"]="05_stored_procedures/V5__create_stored_procedures.sql"
    ["6. Data Initialization/Optima-DataInitialization.sql"]="06_data_initialization/V6__data_initialization.sql"
)

# Create subdirectories
for new_dir in "${DIR_MAPPING[@]}"; do
    mkdir -p "${NEW_SQL_DIR}/${new_dir}"
    echo "✓ Created: sql/${new_dir}"
done
echo ""

# Copy and rename files
echo "Copying and renaming SQL files..."
for old_path in "${!FILE_MAPPING[@]}"; do
    new_path="${FILE_MAPPING[$old_path]}"
    old_file="${BASE_DIR}/${old_path}"
    new_file="${NEW_SQL_DIR}/${new_path}"
    
    if [ -f "$old_file" ]; then
        cp "$old_file" "$new_file"
        echo "✓ Copied: ${old_path} -> sql/${new_path}"
    else
        echo "⚠ Warning: File not found: ${old_path}"
    fi
done
echo ""

# Update flyway.conf if it exists
FLYWAY_CONF="${BASE_DIR}/flyway.conf"
if [ -f "$FLYWAY_CONF" ]; then
    echo "Updating flyway.conf..."
    
    # Backup original
    cp "$FLYWAY_CONF" "${FLYWAY_CONF}.bak"
    
    # Create or update flyway.conf
    cat > "$FLYWAY_CONF" << 'EOF'
# Flyway Configuration File

# JDBC connection settings
# flyway.url=jdbc:sqlserver://localhost:1433;databaseName=YourDatabase
# flyway.user=your_username
# flyway.password=your_password

# Migration locations (Flyway will scan recursively)
flyway.locations=filesystem:sql

# Migration naming patterns
flyway.sqlMigrationPrefix=V
flyway.sqlMigrationSeparator=__
flyway.sqlMigrationSuffixes=.sql

# Baseline settings (if migrating existing database)
# flyway.baselineOnMigrate=true
# flyway.baselineVersion=0

# Schema settings
# flyway.schemas=dbo

# Validation
flyway.validateOnMigrate=true

# Encoding
flyway.encoding=UTF-8
EOF
    
    echo "✓ flyway.conf updated"
    echo "✓ Original backed up as flyway.conf.bak"
else
    echo "⚠ flyway.conf not found, creating new one..."
    cat > "$FLYWAY_CONF" << 'EOF'
# Flyway Configuration File

# JDBC connection settings - CONFIGURE THESE
flyway.url=jdbc:sqlserver://localhost:1433;databaseName=YourDatabase
flyway.user=your_username
flyway.password=your_password

# Migration locations (Flyway will scan recursively)
flyway.locations=filesystem:sql

# Migration naming patterns
flyway.sqlMigrationPrefix=V
flyway.sqlMigrationSeparator=__
flyway.sqlMigrationSuffixes=.sql

# Baseline settings (if migrating existing database)
flyway.baselineOnMigrate=true
flyway.baselineVersion=0

# Schema settings
flyway.schemas=dbo

# Validation
flyway.validateOnMigrate=true

# Encoding
flyway.encoding=UTF-8
EOF
    echo "✓ flyway.conf created"
fi
echo ""

# Create or update run-flyway-migration.sh
RUN_SCRIPT="${BASE_DIR}/run-flyway-migration.sh"
cat > "$RUN_SCRIPT" << 'EOF'
#!/bin/bash

# Flyway Migration Runner Script

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

# Check if flyway is installed
if ! command -v flyway &> /dev/null; then
    echo "Error: Flyway is not installed or not in PATH"
    echo "Please install Flyway: https://flywaydb.org/documentation/usage/commandline/#download-and-installation"
    exit 1
fi

# Display usage
if [ $# -eq 0 ]; then
    echo "Usage: $0 [command]"
    echo ""
    echo "Available commands:"
    echo "  info      - Show migration status"
    echo "  validate  - Validate applied migrations"
    echo "  migrate   - Apply pending migrations"
    echo "  baseline  - Baseline existing database"
    echo "  repair    - Repair migration history"
    echo ""
    exit 0
fi

COMMAND=$1

echo "=== Running Flyway $COMMAND ==="
echo "Configuration: flyway.conf"
echo "SQL Location: sql/"
echo ""

case $COMMAND in
    info)
        flyway -configFiles=flyway.conf info
        ;;
    validate)
        flyway -configFiles=flyway.conf validate
        ;;
    migrate)
        echo "⚠ This will apply database migrations. Continue? (y/n)"
        read -r confirm
        if [ "$confirm" = "y" ]; then
            flyway -configFiles=flyway.conf migrate
            echo ""
            echo "✓ Migration completed successfully"
        else
            echo "Migration cancelled"
        fi
        ;;
    baseline)
        echo "⚠ This will baseline the database. Continue? (y/n)"
        read -r confirm
        if [ "$confirm" = "y" ]; then
            flyway -configFiles=flyway.conf baseline
            echo ""
            echo "✓ Baseline completed successfully"
        else
            echo "Baseline cancelled"
        fi
        ;;
    repair)
        flyway -configFiles=flyway.conf repair
        ;;
    *)
        echo "Unknown command: $COMMAND"
        echo "Run without arguments to see available commands"
        exit 1
        ;;
esac
EOF

chmod +x "$RUN_SCRIPT"
echo "✓ run-flyway-migration.sh created/updated and made executable"
echo ""

# Summary
echo "=== Restructure Complete! ==="
echo ""
echo "New structure created at: ${NEW_SQL_DIR}"
echo "Backup location: ${BACKUP_DIR}"
echo ""
echo "Directory structure:"
tree -L 2 "$NEW_SQL_DIR" 2>/dev/null || find "$NEW_SQL_DIR" -type f -name "*.sql" | sort
echo ""
echo "Next steps:"
echo "1. Review the new sql/ directory structure"
echo "2. Edit flyway.conf and update database connection settings"
echo "3. Test migration: ./run-flyway-migration.sh info"
echo "4. Apply migrations: ./run-flyway-migration.sh migrate"
echo ""
echo "If you want to clean up old directories after verifying:"
echo "  rm -rf '${BASE_DIR}/1. Table & Trigger'"
echo "  rm -rf '${BASE_DIR}/2. TableType'"
echo "  rm -rf '${BASE_DIR}/3. View'"
echo "  rm -rf '${BASE_DIR}/4. Function'"
echo "  rm -rf '${BASE_DIR}/5. StoredProcedure'"
echo "  rm -rf '${BASE_DIR}/6. Data Initialization'"
echo ""