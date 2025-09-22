variable "country_code" {
  description = "Country code (e.g., jp, uae)"
  type        = string
}

variable "country_name" {
  description = "Country name"
  type        = string
}

variable "environment" {
  description = "Environment (dev, staging, production)"
  type        = string
  default     = "demo"
}

variable "aws_region" {
  description = "AWS region"
  type        = string
}

variable "availability_zones" {
  description = "Availability zones"
  type        = list(string)
}

variable "domain_base" {
  description = "Base domain (xva-rnd.com)"
  type        = string
}

variable "ui_subdomain" {
  description = "UI subdomain (optima-jp)"
  type        = string
}

variable "api_subdomain" {
  description = "API subdomain (api-optima-jp)"
  type        = string
}

variable "vpc_cidr" {
  description = "VPC CIDR block"
  type        = string
}

variable "public_subnets" {
  description = "Public subnet CIDR blocks"
  type        = list(string)
}

variable "api_instance_type" {
  description = "API server instance type"
  type        = string
  default     = "t3.micro"
}

variable "ui_instance_type" {
  description = "UI server instance type"
  type        = string
  default     = "t3.micro"
}

variable "db_instance_type" {
  description = "Database server instance type"
  type        = string
  default     = "t3.micro"
}

variable "key_pair_name" {
  description = "EC2 Key Pair name"
  type        = string
}

# Add these to your existing terraform/variables.tf

variable "private_subnets" {
  description = "Private subnet CIDR blocks (for RDS)"
  type        = list(string)
}

# RDS Configuration Variables
variable "db_instance_class" {
  description = "RDS instance class for SQL Server"
  type        = string
  default     = "db.t3.micro"
}

variable "db_engine" {
  description = "Database engine"
  type        = string
  default     = "sqlserver-ex"
}

variable "db_engine_version" {
  description = "SQL Server engine version"
  type        = string
  default     = "15.00.4316.3.v1"
}

variable "db_allocated_storage" {
  description = "Allocated storage in GB for RDS"
  type        = number
  default     = 20
}

variable "db_username" {
  description = "Database master username"
  type        = string
  default     = "optima_admin"
}

variable "db_password" {
  description = "Database master password"
  type        = string
  sensitive   = true
}

variable "domain_name" {
  description = "Base domain name for the application"
  type        = string
  default     = "xva-rnd.com"
}

variable "dns_ttl" {
  description = "DNS record TTL in seconds"
  type        = number
  default     = 300
}