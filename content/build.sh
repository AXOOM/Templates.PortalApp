#!/bin/sh
set -e
cd `dirname $0`

src/build.sh ${1:-0.1-dev}
src/test.sh
