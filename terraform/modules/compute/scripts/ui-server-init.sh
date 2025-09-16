#!/bin/bash
# terraform/modules/compute/scripts/ui-server-init.sh
# Minimal Day 2 Script - Just prove the server works

# Variables from Terraform
COUNTRY_CODE="${country_code}"
ENVIRONMENT="${environment}"
API_HOST="${api_host}"
DB_HOST="${db_host}"

# Convert to uppercase using tr command
COUNTRY_UPPER=$(echo "$COUNTRY_CODE" | tr '[:lower:]' '[:upper:]')
ENV_UPPER=$(echo "$ENVIRONMENT" | tr '[:lower:]' '[:upper:]')

# Update system and install nginx
apt-get update -y
apt-get install -y nginx

# Create simple HTML page
cat > /var/www/html/index.html << EOF
<!DOCTYPE html>
<html>
<head>
    <title>Optima UI - $COUNTRY_UPPER</title>
    <style>
        body { font-family: Arial; text-align: center; margin-top: 100px; background: #f0f8ff; }
        .success { color: green; font-size: 2em; }
    </style>
</head>
<body>
    <h1 class="success">Optima UI Server Ready!</h1>
    <h2>Country: $COUNTRY_UPPER - $ENV_UPPER</h2>
    <p>Server Time: $(date)</p>
    <p>API Server: $API_HOST</p>
    <p>Database: $DB_HOST</p>
    <p>Status: Ubuntu + Nginx Ready for Laravel 10</p>
</body>
</html>
EOF

# Start nginx
systemctl start nginx
systemctl enable nginx

# Configure firewall
ufw --force enable
ufw allow 80/tcp
ufw allow 22/tcp

echo "Optima UI Server $COUNTRY_CODE - $ENVIRONMENT initialized successfully!"