output "vpc_id" {
  description = "VPC ID"
  value       = module.networking.vpc_id
}

output "ui_domain" {
  description = "UI domain name"
  value       = "${var.ui_subdomain}.${var.domain_base}"
}

output "api_domain" {
  description = "API domain name"
  value       = "${var.api_subdomain}.${var.domain_base}"
}

output "public_subnet_ids" {
  description = "Public subnet IDs"
  value       = module.networking.public_subnet_ids
}

output "api_server_public_ip" {
  description = "API server public IP"
  value       = module.compute.api_server_public_ip
}

output "ui_server_public_ip" {
  description = "UI server public IP" 
  value       = module.compute.ui_server_public_ip
}

output "rds_endpoint" {
  description = "RDS database endpoint"
  value       = module.compute.rds_endpoint
  sensitive   = true
}

output "ssh_key_file" {
  description = "SSH private key filename"
  value       = module.compute.private_key_filename
}

output "api_server_id" {
  description = "API server instance ID"
  value       = module.compute.api_server_id
}