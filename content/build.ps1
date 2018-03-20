Param ([string]$Version = "0.1-pre", [Switch]$DeployLocal)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

src\build-dotnet.ps1 $Version
src\test-dotnet.ps1
src\build-docker.ps1 $Version
release\build.ps1 $Version

if ($DeployLocal) {
    0install add-feed --batch release\asset-$Version.xml
    0install run http://assets.axoom.cloud/tools/ax.xml deploy --refresh -f deploy\local.yml --feed http://assets.axoom.cloud/apps/axoom-myapp.xml=$Version
    0install remove-feed --batch release\asset-$Version.xml
}

popd
