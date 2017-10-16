Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"

pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

# Build NuGet Package
nuget pack -Version $Version -OutputDirectory artifacts -NoPackageAnalysis

popd
