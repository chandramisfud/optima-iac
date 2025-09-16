# terraform/modules/compute/outputs.tf
# Outputs from Optima Compute Module - Updated for RDS

# Server IPs
output "api_server_public_ip" {
  description = "Public IP of the API server"
  value       = aws_instance.api_server.public_ip
}

output "api_server_private_ip" {
  description = "Private IP of the API server"
  value       = aws_instance.api_server.private_ip
}

output "ui_server_public_ip" {
  description = "Public IP of the UI server"
  value       = aws_instance.ui_server.public_ip
}

output "ui_server_private_ip" {
  description = "Private IP of the UI server"
  value       = aws_instance.ui_server.private_ip
}

# RDS Database Information
output "rds_endpoint" {
  description = "RDS instance endpoint"
  value       = aws_db_instance.optima.endpoint
}

output "rds_port" {
  description = "RDS instance port"
  value       = aws_db_instance.optima.port
}

output "rds_database_name" {
  description = "Database name"
  value       = aws_db_instance.optima.db_name
}

output "rds_username" {
  description = "Database master username"
  value       = aws_db_instance.optima.username
  sensitive   = true
}

# Instance IDs
output "api_server_id" {
  description = "Instance ID of the API server"
  value       = aws_instance.api_server.id
}

output "ui_server_id" {
  description = "Instance ID of the UI server"
  value       = aws_instance.ui_server.id
}

output "rds_instance_id" {
  description = "RDS instance ID"
  value       = aws_db_instance.optima.id
}

# Key Pair
output "key_pair_name" {
  description = "Name of the created key pair"
  value       = aws_key_pair.optima.key_name
}

# Security Groups
output "api_security_group_id" {
  description = "Security group ID for API server"
  value       = aws_security_group.api_server.id
}

output "ui_security_group_id" {
  description = "Security group ID for UI server"
  value       = aws_security_group.ui_server.id
}

output "rds_security_group_id" {
  description = "Security group ID for RDS database"
  value       = aws_security_group.rds.id
}

# Hostnames for easy access
output "ui_hostname" {
  description = "Planned hostname for UI server"
  value       = "optima-${var.country_code}.${var.domain_base}"
}

output "api_hostname" {
  description = "Planned hostname for API server"
  value       = "api-optima-${var.country_code}.${var.domain_base}"
}

# Connection Information
output "ui_ssh_command" {
  description = "SSH command to connect to UI server"
  value       = "ssh -i ./optima-${var.country_code}-${var.environment}.pem ubuntu@${aws_instance.ui_server.public_ip}"
}

output "private_key_filename" {
  description = "Filename of the generated private key"
  value       = "optima-${var.country_code}-${var.environment}.pem"
}

output "api_rdp_info" {
  description = "RDP connection info for API server"
  value       = {
    host = aws_instance.api_server.public_ip
    port = 3389
    note = "Use Windows credentials from EC2 console"
  }
}

# Database Connection String Template
output "database_connection_info" {
  description = "Database connection information"
  value = {
    server   = aws_db_instance.optima.endpoint
    port     = aws_db_instance.optima.port
    database = aws_db_instance.optima.db_name
    note     = "Use db_username and db_password variables for credentials"
  }
  sensitive = true
}