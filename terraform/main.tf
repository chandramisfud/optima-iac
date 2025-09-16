# Main Terraform configuration for Optima

# VPC and Networking
module "networking" {
  source = "./modules/networking"
  
  country_code       = var.country_code
  environment        = var.environment
  vpc_cidr          = var.vpc_cidr
  public_subnets    = var.public_subnets
  private_subnets   = var.private_subnets # Add this for RDS
  availability_zones = var.availability_zones
}

# Compute instances and RDS
module "compute" {
  source = "./modules/compute"
  
  # Basic config
  country_code      = var.country_code
  environment       = var.environment
  
  # Networking
  vpc_id              = module.networking.vpc_id
  public_subnet_ids   = module.networking.public_subnet_ids
  private_subnet_ids  = module.networking.private_subnet_ids  # For RDS
  
  # Instance types
  api_instance_type = var.api_instance_type
  ui_instance_type  = var.ui_instance_type
  
  # RDS Database config
  db_instance_class    = var.db_instance_class
  db_engine           = var.db_engine
  db_engine_version   = var.db_engine_version
  db_allocated_storage = var.db_allocated_storage
  db_username         = var.db_username
  db_password         = var.db_password
  
  # Domain
  domain_base = var.domain_base
}

# DNS configuration (we'll create this module later)
# module "dns" {
#   source = "./modules/dns"
#   
#   country_code    = var.country_code
#   environment     = var.environment
#   domain_base     = var.domain_base
#   ui_subdomain    = var.ui_subdomain
#   api_subdomain   = var.api_subdomain
#   
#   ui_server_ip  = module.compute.ui_server_public_ip
#   api_server_ip = module.compute.api_server_public_ip
#   rds_endpoint  = module.compute.rds_endpoint
# }