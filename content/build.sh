#!/bin/sh
set -e
cd `dirname $0`/src

# Inject NuGet pull credentials for CI builds
if [ -n "$ARTIFACTORY_API_KEY" ]; then
  sed -i "s/<!--<packageSourceCredentials>/<packageSourceCredentials>/g; s/ARTIFACTORY_USER/$ARTIFACTORY_USER/g; s/ARTIFACTORY_API_KEY/$ARTIFACTORY_API_KEY/g; s/<\/packageSourceCredentials>-->/<\/packageSourceCredentials>/g" NuGet.Config
fi

dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=${1:-0.1-dev} MyVendor.MyApp.sln
dotnet test --no-build --configuration Release --logger "junit;LogFilePath=../../artifacts/test-results.xml" UnitTests/UnitTests.csproj
