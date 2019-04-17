#!/bin/sh
set -e
cd `dirname $0`

if [ ! -d ~/.helm ]; then
  helm init --client-only
fi

helmfile -f myvendor-myservice/helmfile.repos.yaml repos

TENANT_ID="example-tenant" \
PUBLIC_CLUSTER_DOMAIN="example.com" \
helmfile -f myvendor-myservice/helmfile.repos.yaml lint

mkdir -p ../artifacts
helm package --dependency-update --destination ../artifacts --version ${1:-0.1-dev} myvendor-myservice
