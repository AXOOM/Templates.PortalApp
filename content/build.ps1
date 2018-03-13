Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

src\build-dotnet.ps1 $Version
src\test-dotnet.ps1
src\build-docker.ps1 $Version
release\build.ps1 $Version

popd
