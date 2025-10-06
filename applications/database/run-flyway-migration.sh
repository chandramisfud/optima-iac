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

# Check for sqlcmd (needed for database creation)
if ! command -v sqlcmd &> /dev/null; then
    echo "Error: sqlcmd is not installed or not in PATH."
    echo "This script requires the SQL Server command-line tools to ensure the database exists."
    exit 1
fi

# Display usage
if [ $# -eq 0 ]; then
    echo "Usage: $0 [command]"
    echo ""
    echo "Available commands:"
    echo "  info      - Show migration status"
    echo "  validate  - Validate applied migrations"
    echo "  migrate   - Create DB if not exists, then apply pending migrations"
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
        if ! command -v sqlcmd &> /dev/null; then
            echo "Error: 'migrate' command requires sqlcmd to be installed."
            echo "Please run: brew install mssql-tools18"
            exit 1
        fi
        
        echo "⚠ This will ensure the database exists and apply migrations. Continue? (y/n)"
        read -r confirm
        if [ "$confirm" = "y" ]; then
            
            # (The rest of your database creation and migration logic goes here...)
            
            echo ""
            echo "--- Ensuring database exists ---"
            
            DB_NAME=$(grep "flyway.url" flyway.conf | sed -E 's/.*databaseName=([^;]+).*/\1/')
            DB_HOST=$(grep "flyway.url" flyway.conf | sed -E 's/.*:\/\/(.*):.*/\1/')
            DB_PORT=$(grep "flyway.url" flyway.conf | sed -E 's/.*:([0-9]+);.*/\1/')
            DB_USER=$(grep "flyway.user" flyway.conf | cut -d'=' -f2)
            DB_PASSWORD=${FLYWAY_PASSWORD}

            if [ -z "$DB_PASSWORD" ]; then
                echo "Error: FLYWAY_PASSWORD environment variable is not set."
                exit 1
            fi
            
            echo "Checking for database '$DB_NAME' on host '$DB_HOST:$DB_PORT'..."
            CREATE_DB_SCRIPT="IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'${DB_NAME}') BEGIN CREATE DATABASE [${DB_NAME}]; END"
            sqlcmd -S "${DB_HOST},${DB_PORT}" -U "${DB_USER}" -P "${DB_PASSWORD}" -d "master" -Q "${CREATE_DB_SCRIPT}"
            
            echo "Database is ready."
            echo "------------------------------------"
            
            flyway -configFiles=flyway.conf migrate
            echo ""
            echo "✓ Migration completed successfully"
        else
            echo "Migration cancelled"
        fi
        ;;
    # (rest of your script: baseline, repair, etc.)
esac
esac