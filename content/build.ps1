Param ([Parameter(Mandatory=$True)][string]$Version)
$ErrorActionPreference = "Stop"
Push-Location $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

.\src\build-dotnet.ps1 $Version
.\src\build-docker.ps1 $Version
.\release\build.ps1 $Version

Pop-Location
