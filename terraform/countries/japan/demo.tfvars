# Japan Demo Configuration
country_code = "jp"
country_name = "japan"
environment  = "demo"

# AWS Region (Tokyo for Japan)
aws_region = "ap-northeast-1"
availability_zones = ["ap-northeast-1a", "ap-northeast-1c"]

# Domain Configuration
domain_base   = "xva-rnd.com"
ui_subdomain  = "optima-jp"
api_subdomain = "api-optima-jp"

# Network Configuration
vpc_cidr        = "10.0.0.0/16"
public_subnets  = ["10.0.1.0/24", "10.0.2.0/24"]
private_subnets = ["10.0.10.0/24", "10.0.11.0/24"]  # Add this for RDS

# Instance Configuration (Updated for our stack)
api_instance_type = "t3.medium"  # Windows needs more resources
ui_instance_type  = "t3.small"

# RDS Configuration (Add these)
db_instance_class    = "db.t3.micro"
db_engine           = "sqlserver-ex"
db_engine_version   = "15.00.4316.3.v1"
db_allocated_storage = 20
db_username         = "optima_admin"
db_password         = "OptimaDemo2024!"  # Change this to something secure

# Key pair for SSH access
key_pair_name = "optima-demo"