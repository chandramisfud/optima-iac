# terraform/modules/dns/variables.tf

variable "domain_name" {
  description = "Base domain name (e.g., xva-rnd.com)"
  type        = string
}

variable "country_code" {
  description = "Country code for subdomain (e.g., jp, uae)"
  type        = string
}

variable "environment" {
  description = "Environment name (e.g., demo, prod)"
  type        = string
  default     = "demo"
}

variable "ui_server_ip" {
  description = "IP address of the UI server (Laravel)"
  type        = string
}

variable "api_server_ip" {
  description = "IP address of the API server (.NET)"
  type        = string
}

variable "ttl" {
  description = "DNS record TTL in seconds"
  type        = number
  default     = 300
}


