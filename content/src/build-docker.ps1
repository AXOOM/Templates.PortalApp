Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
Push-Location $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$env:DOCKER_REGISTRY = if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"}
$env:VERSION = $Version
docker-compose -f docker-compose.yml build

Pop-Location
