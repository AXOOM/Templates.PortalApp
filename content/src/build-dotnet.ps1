Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

dotnet clean MyVendor.MyApp.sln
dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=$Version MyVendor.MyApp.sln

popd
