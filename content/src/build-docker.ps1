Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

$env:VERSION = $Version
$env:DOCKER_REGISTRY = if ($Version.Contains("-")) {"docker-ci.axoom.cloud"} else {"docker.axoom.cloud"}
docker-compose -f docker-compose.yml build

popd
