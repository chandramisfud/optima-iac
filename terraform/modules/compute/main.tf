# terraform/modules/compute/main.tf
# Optima Multi-Country Compute Infrastructure - Updated for RDS

# Data source for latest Windows Server AMI
data "aws_ami" "windows_server" {
  most_recent = true
  owners      = ["amazon"]
  
  filter {
    name   = "name"
    values = ["Windows_Server-2022-English-Full-Base-*"]
  }
  
  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
}

# Data source for latest Ubuntu AMI
data "aws_ami" "ubuntu" {
  most_recent = true
  owners      = ["099720109477"] # Canonical
  
  filter {
    name   = "name"
    values = ["ubuntu/images/hvm-ssd/ubuntu-jammy-22.04-amd64-server-*"]
  }
  
  filter {
    name   = "virtualization-type"
    values = ["hvm"]
  }
  
  filter {
    name   = "state"
    values = ["available"]
  }
}

# Generate SSH key pair
resource "tls_private_key" "optima" {
  algorithm = "RSA"
  rsa_bits  = 2048
}

resource "aws_key_pair" "optima" {
  key_name   = "optima-${var.country_code}-${var.environment}"
  public_key = tls_private_key.optima.public_key_openssh
  
  tags = {
    Name        = "optima-${var.country_code}-${var.environment}"
    Country     = var.country_code
    Environment = var.environment
  }
}

# Save private key to file for SSH access
resource "local_file" "private_key" {
  content         = tls_private_key.optima.private_key_pem
  filename        = "${path.root}/optima-${var.country_code}-${var.environment}.pem"
  file_permission = "0600"
}

# Security Groups
resource "aws_security_group" "api_server" {
  name_prefix = "optima-${var.country_code}-${var.environment}-api-"
  vpc_id      = var.vpc_id
  description = "Security group for Optima API Server (.NET 7/Windows)"
  
  # RDP access for management
  ingress {
    from_port   = 3389
    to_port     = 3389
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # For demo only - restrict in production
  }
  
  # HTTP for API
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  # HTTPS for API
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  # Custom API port
  ingress {
    from_port   = 8080
    to_port     = 8080
    protocol    = "tcp"
    cidr_blocks = ["10.0.0.0/16"] # VPC only
  }
  ingress {
    from_port   = 5986
    to_port     = 5986
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # For demo only - restrict in production
  }
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  tags = {
    Name = "optima-${var.country_code}-${var.environment}-api-sg"
  }
}

resource "aws_security_group" "ui_server" {
  name_prefix = "optima-${var.country_code}-${var.environment}-ui-"
  vpc_id      = var.vpc_id
  description = "Security group for Optima UI Server (Laravel/Ubuntu)"
  
  # SSH access
  ingress {
    from_port   = 22
    to_port     = 22
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"] # For demo only
  }
  
  # HTTP
  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  # HTTPS
  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  tags = {
    Name = "optima-${var.country_code}-${var.environment}-ui-sg"
  }
}

resource "aws_security_group" "rds" {
  name_prefix = "optima-${var.country_code}-${var.environment}-rds-"
  vpc_id      = var.vpc_id
  description = "Security group for Optima RDS SQL Server"
  
  # SQL Server from API server
  ingress {
    from_port       = 1433
    to_port         = 1433
    protocol        = "tcp"
    security_groups = [aws_security_group.api_server.id]
  }
  
  # SQL Server from UI server (for direct queries if needed)
  ingress {
    from_port       = 1433
    to_port         = 1433
    protocol        = "tcp"
    security_groups = [aws_security_group.ui_server.id]
  }
  
  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
  
  tags = {
    Name = "optima-${var.country_code}-${var.environment}-rds-sg"
  }
}

# RDS Subnet Group
resource "aws_db_subnet_group" "optima" {
  name       = "optima-${var.country_code}-${var.environment}"
  subnet_ids = var.private_subnet_ids
  
  tags = {
    Name        = "optima-${var.country_code}-${var.environment}-db-subnet-group"
    Country     = var.country_code
    Environment = var.environment
  }
}

# RDS SQL Server Instance
resource "aws_db_instance" "optima" {
  identifier = "optima-${var.country_code}-${var.environment}"
  
  # Engine configuration
  engine         = var.db_engine
  engine_version = var.db_engine_version
  instance_class = var.db_instance_class
  
  # Storage configuration
  allocated_storage     = var.db_allocated_storage
  max_allocated_storage = var.db_allocated_storage * 2 # Auto scaling up to 2x
  storage_type         = "gp2"
  storage_encrypted    = true
  
  # Database configuration
#   db_name  = "optima_${var.country_code}"
  username = var.db_username
  password = var.db_password
  
  # Network configuration
  db_subnet_group_name   = aws_db_subnet_group.optima.name
  vpc_security_group_ids = [aws_security_group.rds.id]
  publicly_accessible    = false
  
  # Backup configuration
  backup_retention_period = 7
  backup_window          = "03:00-04:00"
  maintenance_window     = "sun:04:00-sun:05:00"
  
  # Monitoring
#   monitoring_interval = 60
# monitoring_role_arn = aws_iam_role.rds_monitoring.arn
  
  # Other settings
  skip_final_snapshot = true # For demo only - set to false in production
  deletion_protection = false # For demo only - set to true in production
  
  tags = {
    Name        = "optima-${var.country_code}-${var.environment}-rds"
    Country     = var.country_code
    Environment = var.environment
  }
}

# IAM role for RDS monitoring
# resource "aws_iam_role" "rds_monitoring" {
#   name = "optima-${var.country_code}-${var.environment}-rds-monitoring"

#   assume_role_policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [
#       {
#         Action = "sts:AssumeRole"
#         Effect = "Allow"
#         Principal = {
#           Service = "monitoring.rds.amazonaws.com"
#         }
#       }
#     ]
#   })
# }

# resource "aws_iam_role_policy_attachment" "rds_monitoring" {
#   role       = aws_iam_role.rds_monitoring.name
#   policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonRDSEnhancedMonitoringRole"
# }

# API Server (Windows/.NET 7)
resource "aws_instance" "api_server" {
  ami                     = data.aws_ami.windows_server.id
  instance_type           = var.api_instance_type
  subnet_id               = var.public_subnet_ids[0]
  vpc_security_group_ids  = [aws_security_group.api_server.id]
  key_name                = aws_key_pair.optima.key_name
  associate_public_ip_address = true
  
  # User data for Windows initial setup
  user_data = base64encode(templatefile("${path.module}/scripts/api-server-init.ps1", {
    country_code = var.country_code
    environment  = var.environment
    db_host     = aws_db_instance.optima.endpoint
    db_name     = aws_db_instance.optima.db_name
    db_user     = var.db_username
    db_password = var.db_password
  }))
  
  root_block_device {
    volume_type           = "gp3"
    volume_size           = 50
    delete_on_termination = true
    encrypted             = true
  }
  
  tags = {
    Name        = "optima-${var.country_code}-${var.environment}-api"
    Country     = var.country_code
    Environment = var.environment
    Role        = "api-server"
    OS          = "windows"
  }
  
  depends_on = [aws_db_instance.optima]
}

# UI Server (Ubuntu/Laravel)
resource "aws_instance" "ui_server" {
  ami                     = data.aws_ami.ubuntu.id
  instance_type           = var.ui_instance_type
  subnet_id               = var.public_subnet_ids[1]
  vpc_security_group_ids  = [aws_security_group.ui_server.id]
  key_name                = aws_key_pair.optima.key_name
  associate_public_ip_address = true
  
  # User data for Ubuntu initial setup
  user_data = base64encode(templatefile("${path.module}/scripts/ui-server-init.sh", {
    country_code = var.country_code
    environment  = var.environment
    api_host     = aws_instance.api_server.private_ip
    db_host     = aws_db_instance.optima.endpoint
    db_name     = aws_db_instance.optima.db_name
    db_user     = var.db_username
    db_password = var.db_password
    domain_base  = var.domain_base
    subdomain    = "optima-${var.country_code}"
  }))
  
  root_block_device {
    volume_type           = "gp3"
    volume_size           = 20
    delete_on_termination = true
    encrypted             = true
  }
  
  tags = {
    Name        = "optima-${var.country_code}-${var.environment}-ui"
    Country     = var.country_code
    Environment = var.environment
    Role        = "ui-server"
    OS          = "ubuntu"
  }
  
  depends_on = [aws_instance.api_server, aws_db_instance.optima]
}