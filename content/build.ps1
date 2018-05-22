Param ([string]$Version = "0.1-pre", [Switch]$DeployLocal, [Switch]$DebugOverride)
$ErrorActionPreference = "Stop"
pushd $(Split-Path -Path $MyInvocation.MyCommand.Definition -Parent)

if (!$DebugOverride) {
    src\build-dotnet.ps1 $Version
    src\test-dotnet.ps1
    src\build-docker.ps1 $Version
}
release\build.ps1 $Version

if ($DeployLocal) {
    0install add-feed --batch release\asset-$Version.xml
    0install run http://assets.axoom.cloud/tools/ax.xml deploy --refresh -f deploy\local.yml --feed http://assets.axoom.cloud/apps/myvendor-myapp.xml=$Version `
        $(if ($DebugOverride) {"--compose-override=deploy\local.docker-compose.override.yml"})
    0install remove-feed --batch release\asset-$Version.xml
    Start-Process "http://myinstance.local.myaxoom.eu"
}

popd
