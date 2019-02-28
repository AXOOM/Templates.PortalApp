# My App Helm Chart

This Helm chart deploys My App.

my description

Use [Quberneeds](https://github.com/AXOOM/Quberneeds) to deploy this chart with the following environment variables:

| Name                     | Default      | Description                                                                                |
|--------------------------|--------------|--------------------------------------------------------------------------------------------|
| `TENANT_ID`              | (required)   | The Kubernetes namespace to deploy to.                                                     |
| `PUBLIC_CLUSTER_DOMAIN`  | (required)   | The public DNS prefix the Kubernetes cluster is exposed under.                             |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Set to `Development` to enable debug logging, Swagger documentation, exception pages, etc. |
