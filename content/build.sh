#!/bin/sh
set -e
cd `dirname $0`

src/build-dotnet.sh ${1:-0.1-dev}
src/test-dotnet.sh
src/build-docker.sh ${1:-0.1-dev}
charts/build.sh ${1:-0.1-dev}
