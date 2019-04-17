Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

src\build-dotnet.ps1 $Version
src\test-dotnet.ps1
src\build-docker.ps1 $Version
charts\build.ps1 $Version

popd
