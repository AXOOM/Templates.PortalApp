$ErrorActionPreference = "Stop"
pushd $PSScriptRoot

dotnet test --no-build --configuration Release --logger "junit;LogFilePath=..\..\artifacts\test-results.xml" UnitTests\UnitTests.csproj

popd
