Param ([string]$Version = "0.1-dev")
$ErrorActionPreference = "Stop"
pushd $PSScriptRoot\src

dotnet clean MyVendor.MyApp.sln

dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=$Version MyVendor.MyApp.sln
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

dotnet test --no-build --configuration Release --logger "junit;LogFilePath=..\..\artifacts\test-results.xml" UnitTests\UnitTests.csproj
if ($LASTEXITCODE -ne 0) {throw "Exit Code: $LASTEXITCODE"}

popd
