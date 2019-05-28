#!/bin/sh
set -e
cd `dirname $0`

dotnet test --no-build --configuration Release --logger "junit;LogFilePath=../../artifacts/test-results.xml" UnitTests/UnitTests.csproj
