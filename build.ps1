Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"

pushd $PSScriptRoot

# Build NuGet Package
nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis

popd
