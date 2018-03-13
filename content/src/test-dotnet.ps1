Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

dotnet test --configuration Release --no-build Axoom.MyApp.UnitTests/Axoom.MyApp.UnitTests.csproj

popd
