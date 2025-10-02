#!/bin/bash
#
# SETUP SCRIPT: Prepares the Linux UI server to act as the Ansible Control Node.
# This bypasses local macOS Python issues.
#
set -e

echo "✅ Preparing UI Server to run Ansible..."

# --- Configuration ---
# IPs from your latest Terraform apply
UI_SERVER_IP="3.112.50.5"
API_SERVER_PRIVATE_IP="10.0.1.189" # Using the private IP is more secure and reliable

# SSH key used to connect to the UI server
SSH_KEY="../terraform/optima-jp-demo.pem"
SSH_USER="ubuntu"

# --- Script Logic ---
echo "▶️ Step 1: Copying Ansible project to the UI server..."
# This command archives your local ansible directory and copies it to the UI server.
tar czf - --exclude="ansible_env" . | ssh -i "$SSH_KEY" "$SSH_USER@$UI_SERVER_IP" 'tar xzf - -C /home/ubuntu'

echo "▶️ Step 2: Installing Ansible and dependencies on the UI server..."
# This runs the installation commands remotely on the UI server.
ssh -i "$SSH_KEY" "$SSH_USER@$UI_SERVER_IP" << 'ENDSSH'
    set -e
    sudo apt-get update
    sudo apt-get install -y python3-pip python3-venv git
    python3 -m venv ansible_env
    source ansible_env/bin/activate
    pip install ansible pywinrm
    echo "✅ Ansible installed successfully in a virtual environment."
ENDSSH

echo "▶️ Step 3: Configuring the inventory on the UI server to use the private IP..."
# This command uses 'sed' to replace the public IP with the private IP in the hosts file on the remote server.
ssh -i "$SSH_KEY" "$SSH_USER@$UI_SERVER_IP" "sed -i 's/18.183.220.135/$API_SERVER_PRIVATE_IP/' /home/ubuntu/inventories/demo/hosts.yml"

echo ""
echo "🎉 SETUP COMPLETE! 🎉"
echo ""
echo "------------------------------------------------------------------"
echo ">>> Your next step is to SSH into the UI server to run commands."
echo ">>> Use this command:"
echo ""
echo "    ssh -i $SSH_KEY $SSH_USER@$UI_SERVER_IP"
echo ""
echo "Once you are logged in, run the win_ping test:"
echo "    cd /home/ubuntu"
echo "    source ansible_env/bin/activate"
echo "    ansible -i inventories/demo/hosts.yml api_servers -m win_ping"
echo "------------------------------------------------------------------"
