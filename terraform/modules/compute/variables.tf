# terraform/modules/compute/variables.tf
# Variables for Optima Compute Module - Updated for RDS

variable "country_code" {
  description = "Country code (e.g., jp, id, sg)"
  type        = string
}

variable "environment" {
  description = "Environment name (dev, staging, prod)"
  type        = string
}

variable "vpc_id" {
  description = "VPC ID where instances will be deployed"
  type        = string
}

variable "public_subnet_ids" {
  description = "List of public subnet IDs for API and UI servers"
  type        = list(string)
}

variable "private_subnet_ids" {
  description = "List of private subnet IDs for RDS database"
  type        = list(string)
}

# Instance Types
variable "api_instance_type" {
  description = "Instance type for API server (Windows)"
  type        = string
  default     = "t3.medium" # Windows needs more resources
}

variable "ui_instance_type" {
  description = "Instance type for UI server (Ubuntu)"
  type        = string
  default     = "t3.small"
}

# RDS Configuration
variable "db_instance_class" {
  description = "RDS instance class for SQL Server"
  type        = string
  default     = "db.t3.small" # RDS instance class format
}

variable "db_engine" {
  description = "Database engine"
  type        = string
  default     = "sqlserver-ex" # SQL Server Express (free tier eligible)
}

variable "db_engine_version" {
  description = "SQL Server engine version"
  type        = string
  default     = "15.00.4316.3.v1" # SQL Server 2019 latest
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

variable "domain_base" {
  description = "Base domain for the deployment (e.g., xva-rnd.com)"
  type        = string
  default     = "xva-rnd.com"
}

variable "windows_password" {
  description = "The administrator password for the Windows server."
  type        = string
  sensitive   = true
}