#!/bin/bash
# Health check for deployed servers

echo "Checking Optima servers..."

# Get server IPs from terraform
UI_IP=$(terraform -chdir=../terraform output -raw ui_server_public_ip)
API_IP=$(terraform -chdir=../terraform output -raw api_server_public_ip)

echo "UI Server ($UI_IP): "
curl -s -o /dev/null -w "%{http_code}\n" http://$UI_IP/ || echo "Failed"

echo "API Server ($API_IP): "
curl -s -o /dev/null -w "%{http_code}\n" http://$API_IP/ || echo "Failed"
