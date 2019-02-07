Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

if (!(Test-Path ~\.helm)) {helm init --client-only}
helmfile -f myvendor-myapp\helmfile.repos.yaml repos
if (!(Test-Path ..\artifacts)) {mkdir ..\artifacts | Out-Null}
helm package --dependency-update --destination ..\artifacts --version $Version myvendor-myapp

popd
