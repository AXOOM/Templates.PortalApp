Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
Push-Location $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$dockerRegistry = $(if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"})
0install run http://assets.axoom.cloud/tools/ax.xml release --verbose --refresh asset.yml $Version `
--arg DOCKER_REGISTRY=$dockerRegistry `
--arg PORTAL_APP=@app.json `
--arg IDENTITY_CLIENT=@client.json `
--arg API_RESOURCES=@apiresources.json `
--verbose `
--refresh

Pop-Location
