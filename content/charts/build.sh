#!/bin/sh
set -e
cd `dirname $0`

if [ ! -d ~/.helm ]; then
  helm init --client-only
fi
helmfile -f myvendor-myapp/helmfile.repos.yaml repos
mkdir -p ../artifacts
helm package --dependency-update --destination ../artifacts --version ${1:-0.1-dev} myvendor-myapp
