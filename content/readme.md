# My App

This repository contains the source code for My App.

## Development

Run `build.ps1` to compile the source code and package the result in Docker images.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).
The `release` directory contains an [ax Asset Descriptor](https://tfs.inside-axoom.org/tfs/axoom/axoom/_git/Axoom.Provisioning?_a=readme&fullScreen=true) for building releases for production Docker environments.

For local testing you must first deploy the [Infrastructure Stack](https://tfs.inside-axoom.org/tfs/axoom/axoom/_git/Axoom.Platform.Stacks.Infrastructure) on your machine.

To build and test without a debugger:

    .\build.ps1 -DeployLocal

To build and test with a debugger:

    .\build.ps1 -DeployLocal -DebugOverride
    # Open src\Axoom.MyApp.sln in Visual Studio and run

 * Portal: http://myinstance.local.mayoom.eu/
 * My App: http://axoom-myapp-myinstance.local.myaxoom.eu/
 * My App API: http://axoom-myapp-myinstance.local.myaxoom.eu/swagger/

## Deploying

### Feed URI

http://assets.axoom.cloud/apps/axoom-myapp.xml

### External environment

| Name | Default | Description |
| ---- | ------- | ----------- |
|      |         |             |
