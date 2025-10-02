# terraform/modules/compute/scripts/api-server-init.ps1
# Variables from Terraform
$CountryCode = "${country_code}"
$Environment = "${environment}"
$AdminPassword = "${admin_password}"

# --- Set Administrator Password ---
# This makes the setup deterministic and avoids fetching random passwords.
# It ensures the password matches what Ansible expects.
Write-Host "Setting Administrator password..."
$user = [adsi]"WinNT://localhost/Administrator,user"
$user.SetPassword($AdminPassword)
$user.SetInfo()
Write-Host "Administrator password has been set."

# --- WinRM Configuration ---
Write-Host "Configuring WinRM for Ansible..."
# Create a self-signed certificate for the WinRM HTTPS listener
$cert = New-SelfSignedCertificate -DnsName "winrm-ansible" -CertStoreLocation "cert:\LocalMachine\My"
$thumbprint = $cert.Thumbprint

# Enable WinRM and create the HTTPS listener
winrm create winrm/config/Listener?Address=*+Transport=HTTPS "@{Hostname=`"winrm-ansible`"; CertificateThumbprint=`"$thumbprint`"}"

# Configure WinRM service options
winrm set winrm/config/service '@{AllowUnencrypted="false"}'
winrm set winrm/config/service/auth '@{Basic="true"}'

# Open the firewall port for WinRM HTTPS
New-NetFirewallRule -DisplayName "Windows Remote Management (HTTPS-In)" -Name "Windows Remote Management (HTTPS-In)" -Profile Any -LocalPort 5986 -Protocol TCP
Write-Host "WinRM configuration complete."

# --- Basic HTML Page for Health Check ---
$htmlContent = @"
<!DOCTYPE html>
<html>
<body>
    <h1>Optima API Server Ready!</h1>
    <p>Status: WinRM configured and ready for Ansible.</p>
</body>
</html>
"@
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot" -Force
$htmlContent | Out-File -FilePath "C:\inetpub\wwwroot\index.html" -Encoding UTF8

Write-Host "Optima API Server $CountryCode - $Environment initialized successfully!"
