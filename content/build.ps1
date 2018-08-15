Param ([string]$Version = "0.1-pre")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

src\build-dotnet.ps1 $Version
src\test-dotnet.ps1
src\build-docker.ps1 $Version

popd
