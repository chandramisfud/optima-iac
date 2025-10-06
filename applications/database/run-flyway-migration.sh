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
