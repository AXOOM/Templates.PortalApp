# My App Helm Chart

This Helm chart deploys My App.

my description

Use [Quberneeds](https://github.com/AXOOM/Quberneeds) to deploy this chart with the following environment variables:

| Name                     | Default       | Description                                                                                                                |
|--------------------------|---------------|----------------------------------------------------------------------------------------------------------------------------|
| `TENANT_ID`              | (required)    | The ID of the customer instance (used as the Kubernetes namespace).                                                        |
| `PUBLIC_DOMAIN`          | (required)    | The public DNS prefix the entire namespace is exposed under.                                                               |
| `LOG_LEVEL`              | `Information` | The [.NET Core Log Level](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?tabs=aspnetcore2x#log-level). |
| `ASPNETCORE_ENVIRONMENT` | `Production`  | Set to `Development` to enable Swagger documentation, exception pages, etc.                                                |
