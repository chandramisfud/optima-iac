# terraform/modules/compute/scripts/api-server-init.ps1
# Variables from Terraform (use lowercase names that match Terraform)
$CountryCode = "${country_code}"
$Environment = "${environment}"
$DbHost = "${db_host}"
$DbName = "${db_name}"
$DbUser = "${db_user}"
$DbPassword = "${db_password}"

# Create basic HTML page
$htmlContent = @"
<!DOCTYPE html>
<html>
<head>
    <title>Optima API - $($CountryCode.ToUpper())</title>
    <style>
        body { font-family: Arial; text-align: center; margin-top: 100px; background: #f0f8ff; }
        .success { color: green; font-size: 2em; }
    </style>
</head>
<body>
    <h1 class="success">✅ Optima API Server Ready!</h1>
    <h2>Country: $($CountryCode.ToUpper()) - $($Environment.ToUpper())</h2>
    <p>Server Time: $(Get-Date)</p>
    <p>Database: $DbHost</p>
    <p>Status: Windows Server + IIS Ready for .NET 7</p>
</body>
</html>
"@

# Create directory and write file
New-Item -ItemType Directory -Path "C:\inetpub\wwwroot" -Force
$htmlContent | Out-File -FilePath "C:\inetpub\wwwroot\index.html" -Encoding UTF8

Write-Host "Optima API Server $CountryCode - $Environment initialized successfully!"