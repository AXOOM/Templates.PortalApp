#!/bin/sh
set -e
cd `dirname $0`

# Inject NuGet pull credentials for CI builds
if [ -n "$ARTIFACTORY_API_KEY" ]; then
  sed "s/ARTIFACTORY_USER/$ARTIFACTORY_USER/g; s/ARTIFACTORY_API_KEY/$ARTIFACTORY_API_KEY/g" NuGet.Template.Config > NuGet.Config
fi

dotnet clean MyVendor.MyApp.sln
dotnet msbuild /t:Restore /t:Build /t:Publish /p:PublishDir=./obj/Docker/publish /p:Configuration=Release /p:Version=${1:-0.1-dev} MyVendor.MyApp.sln
