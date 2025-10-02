#!/bin/bash

# This script retrieves and decrypts the Administrator password for a new Windows EC2 instance.
# It automatically retries until the password is available or a timeout is reached.

# --- Configuration ---
AWS_REGION="ap-northeast-1"
INSTANCE_ID="i-0cebf7cd4333b6061"
PRIVATE_KEY_FILE="../terraform/optima-jp-demo.pem"

# --- Retry Logic ---
MAX_RETRIES=10 # Total wait time = 10 * 30s = 5 minutes
RETRY_INTERVAL=30 # seconds
# --- End of Configuration ---


# Check if AWS CLI is installed
if ! command -v aws &> /dev/null
then
    echo "AWS CLI could not be found. Please install it to continue."
    exit 1
fi

echo "Attempting to fetch password for instance: $INSTANCE_ID. This may take several minutes..."

ENCRYPTED_PASSWORD=""
for (( i=1; i<=MAX_RETRIES; i++ ))
do
    ENCRYPTED_PASSWORD=$(aws ec2 get-password-data \
        --instance-id "$INSTANCE_ID" \
        --priv-launch-key "$PRIVATE_KEY_FILE" \
        --query 'PasswordData' \
        --output text \
        --region "$AWS_REGION" 2>/dev/null)

    if [[ -n "$ENCRYPTED_PASSWORD" && "$ENCRYPTED_PASSWORD" != "None" ]]; then
        echo "Encrypted password received successfully."
        break
    fi

    echo "Password not yet available. Waiting ${RETRY_INTERVAL}s... (Attempt $i of $MAX_RETRIES)"
    sleep $RETRY_INTERVAL
    ENCRYPTED_PASSWORD="" # Reset for the next loop
done


# Check if we timed out
if [ -z "$ENCRYPTED_PASSWORD" ]; then
    echo ""
    echo "Error: Timed out waiting for password data after $(($MAX_RETRIES * $RETRY_INTERVAL / 60)) minutes."
    echo "Please check the AWS console to ensure the instance is running and healthy."
    exit 1
fi

echo "Decrypting with OpenSSL..."

# Use the modern 'pkeyutl' command for decryption
DECRYPTED_PASSWORD=$(echo "$ENCRYPTED_PASSWORD" | base64 --decode | openssl pkeyutl -decrypt -inkey "$PRIVATE_KEY_FILE" -pkeyopt rsa_pkcs1_padding)

echo "--------------------------------------------------"
echo "✅ Success! Your new Windows Administrator password is:"
echo ""
echo "   $DECRYPTED_PASSWORD"
echo ""
echo "--------------------------------------------------"
echo "Next steps:"
echo "1. Update this password in your 'ansible/inventories/demo/group_vars/vault.yml' file."
echo "2. Re-encrypt the vault file with 'ansible-vault encrypt ...'"

