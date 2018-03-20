Param ([string]$Version = "0.1-pre")
$ErrorActionPreference = "Stop"

pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

# Build NuGet Package
nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis

popd
