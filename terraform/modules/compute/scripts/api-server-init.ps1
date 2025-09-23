# terraform/modules/compute/scripts/api-server-init.ps1
# Variables from Terraform
$CountryCode = "${country_code}"
$Environment = "${environment}"

# --- WinRM Configuration ---
# Create a self-signed certificate for the WinRM HTTPS listener
$cert = New-SelfSignedCertificate -DnsName "winrm" -CertStoreLocation "cert:\LocalMachine\My"
$thumbprint = $cert.Thumbprint

# Enable WinRM and create the HTTPS listener
winrm create winrm/config/Listener?Address=*+Transport=HTTPS "@{Hostname=`"winrm`"; CertificateThumbprint=`"$thumbprint`"}"

# Configure WinRM service options
winrm set winrm/config/service '@{AllowUnencrypted="false"}'
winrm set winrm/config/service/auth '@{Basic="true"}'

# Open the firewall port for WinRM HTTPS
New-NetFirewallRule -DisplayName "Windows Remote Management (HTTPS-In)" -Name "Windows Remote Management (HTTPS-In)" -Profile Any -LocalPort 5986 -Protocol TCP

# --- Basic HTML Page for Health Check ---
$htmlContent = @"
<!DOCTYPE html>
<html>
<head>
    <title>Optima API - $($CountryCode.ToUpper())</title>
    <style>
        body { font-family: Arial; text-align: center; margin-top: 100px; }
        .success { color: green; font-size: 2em; }
    </style>
</head>
<body>
    <h1 class="success">✅ Optima API Server Ready!</h1>
    <h2>Country: $($CountryCode.ToUpper()) - $($Environment.ToUpper())</h2>
    <p>Status: WinRM configured and ready for Ansible.</p>
</body>
</html>
"@

# Create directory and write file
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot" -Force
$htmlContent | Out-File -FilePath "C:\inetpub\wwwroot\index.html" -Encoding UTF8

Write-Host "Optima API Server $CountryCode - $Environment initialized and WinRM configured successfully!"