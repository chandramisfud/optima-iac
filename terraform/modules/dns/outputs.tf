# terraform/modules/dns/outputs.tf

output "ui_domain" {
  description = "Full domain name for UI server"
  value       = local.ui_domain
}

output "api_domain" {
  description = "Full domain name for API server"
  value       = local.api_domain
}

output "ui_dns_name" {
  description = "DNS record name for UI server"
  value       = aws_route53_record.ui_server.name
}

output "api_dns_name" {
  description = "DNS record name for API server"
  value       = aws_route53_record.api_server.name
}

output "hosted_zone_id" {
  description = "Route 53 hosted zone ID"
  value       = data.aws_route53_zone.main.zone_id
}

output "ui_health_check_id" {
  description = "Health check ID for UI server"
  value       = aws_route53_health_check.ui_server.id
}

output "api_health_check_id" {
  description = "Health check ID for API server"
  value       = aws_route53_health_check.api_server.id
}