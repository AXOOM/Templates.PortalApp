Param ([string]$Version = "0.1-pre")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

# Ensure 0install is in the PATH
if (!(Get-Command 0install -ErrorAction SilentlyContinue)) {
    mkdir -Force "$env:TEMP\zero-install" | Out-Null
    Invoke-WebRequest "https://0install.de/files/0install.exe" -OutFile "$env:TEMP\zero-install\0install.exe"
    $env:PATH = "$env:TEMP\zero-install;$env:PATH"
}

$dockerRegistry = $(if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"})
0install run http://assets.axoom.cloud/tools/ax.xml release --verbose --refresh asset.yml $Version `
--arg DOCKER_REGISTRY=$dockerRegistry `
--arg PORTAL_APP=@app.json `
--arg IDENTITY_CLIENT=@client.json `
--arg API_RESOURCES=@apiresources.json

popd
