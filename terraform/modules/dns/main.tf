# terraform/modules/dns/main.tf

terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 5.0"
    }
  }
}

# Data source for existing hosted zone
data "aws_route53_zone" "main" {
  name         = var.domain_name
  private_zone = false
}

# Local values for domain construction
locals {
  ui_domain  = "optima-${var.country_code}.${var.domain_name}"
  api_domain = "api-optima-${var.country_code}.${var.domain_name}"
}

# A record for UI server (Laravel)
resource "aws_route53_record" "ui_server" {
  zone_id = data.aws_route53_zone.main.zone_id
  name    = local.ui_domain
  type    = "A"
  ttl     = var.ttl
  records = [var.ui_server_ip]
}

# A record for API server (.NET)
resource "aws_route53_record" "api_server" {
  zone_id = data.aws_route53_zone.main.zone_id
  name    = local.api_domain
  type    = "A"
  ttl     = var.ttl
  records = [var.api_server_ip]
}

# Health check for UI server (optional but good practice)
resource "aws_route53_health_check" "ui_server" {
  fqdn                            = local.ui_domain
  port                            = 80
  type                            = "HTTP"
  resource_path                   = "/"
  failure_threshold               = 5
  request_interval                = 30
  insufficient_data_health_status = "LastKnownStatus"

  tags = {
    Name        = "${var.environment}-${var.country_code}-ui-health"
    Environment = var.environment
    Country     = var.country_code
    Project     = "optima-iac"
  }
}

# Health check for API server (optional)
resource "aws_route53_health_check" "api_server" {
  fqdn                            = local.api_domain
  port                            = 80
  type                            = "HTTP"
  resource_path                   = "/health"
  failure_threshold               = 5
  request_interval                = 30
  insufficient_data_health_status = "LastKnownStatus"

  tags = {
    Name        = "${var.environment}-${var.country_code}-api-health"
    Environment = var.environment
    Country     = var.country_code
    Project     = "optima-iac"
  }
}
