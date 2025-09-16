#!/bin/bash
# Deploy Optima demo environment

set -e

echo "Deploying Optima Demo Environment..."

# Check if terraform outputs are available
if ! terraform -chdir=../terraform output > /dev/null 2>&1; then
    echo "Error: Terraform infrastructure not deployed"
    exit 1
fi

# Run ansible playbook
ansible-playbook playbooks/site.yml

echo "Deployment completed successfully!"
