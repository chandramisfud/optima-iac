#!/bin/bash

# Script to convert versioned view migrations (V) to repeatable migrations (R)
# This allows views to be recreated on every migration run

set -e

echo "=== Converting View Migrations from V to R ==="
echo ""

VIEWS_DIR="sql/03_views"

# Check if directory exists
if [ ! -d "$VIEWS_DIR" ]; then
    echo "Error: Directory '$VIEWS_DIR' not found!"
    exit 1
fi

# Create backup
BACKUP_DIR="${VIEWS_DIR}_backup_$(date +%Y%m%d_%H%M%S)"
echo "Creating backup at: $BACKUP_DIR"
cp -r "$VIEWS_DIR" "$BACKUP_DIR"
echo "✓ Backup created successfully"
echo ""

# Counter
count=0

# Find all V*.sql files and rename to R__
echo "Renaming files from V to R..."
find "$VIEWS_DIR" -name "V*.sql" -type f | sort | while read filepath; do
    dir=$(dirname "$filepath")
    filename=$(basename "$filepath")
    
    # Extract description (everything after V{version}__)
    # V2.23__vw_account_blitz.sql -> vw_account_blitz.sql
    if [[ $filename =~ ^V[0-9.]+__(.+)$ ]]; then
        description="${BASH_REMATCH[1]}"
        newname="R__${description}"
        
        echo "  $filename -> $newname"
        mv "$filepath" "$dir/$newname"
        ((count++))
    else
        echo "  ⚠ Skipping (doesn't match pattern): $filename"
    fi
done

echo ""
echo "✓ Renamed $count files"
echo ""

# Update each R__ file to use CREATE OR ALTER instead of CREATE
echo "Updating SQL files to use CREATE OR ALTER VIEW..."
find "$VIEWS_DIR" -name "R__*.sql" -type f | while read filepath; do
    filename=$(basename "$filepath")
    
    # Check if file contains CREATE VIEW
    if grep -q "CREATE VIEW" "$filepath"; then
        # Replace CREATE VIEW with CREATE OR ALTER VIEW
        sed -i '' 's/CREATE VIEW/CREATE OR ALTER VIEW/g' "$filepath"
        echo "  ✓ Updated: $filename"
    fi
done

echo ""
echo "=== Conversion Complete! ==="
echo ""
echo "Summary:"
echo "- Backup location: $BACKUP_DIR"
echo "- Files renamed: $count"
echo "- All CREATE VIEW changed to CREATE OR ALTER VIEW"
echo ""
echo "Repeatable migrations (R__) will run every time you execute 'flyway migrate'"
echo "This is perfect for views, as they can be safely recreated."
echo ""
echo "Next steps:"
echo "1. Review the renamed files in: $VIEWS_DIR"
echo "2. Reset Flyway: flyway -configFiles=flyway.conf -cleanDisabled=false clean"
echo "3. Run migration: ./run-flyway-migration.sh migrate"
echo ""