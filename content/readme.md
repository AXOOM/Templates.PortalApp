# My App

This repository contains the source code for My App.

Run `build.ps1` to compile the source code and package the result in Docker images.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).
The `release` directory contains an [ax Asset Descriptor](https://tfs.inside-axoom.org/tfs/axoom/axoom/_git/Axoom.Provisioning?_a=readme&fullScreen=true) for building releases for production Docker environments.

For local testing:

    cd src
    ./build-dotnet.ps1
    docker-compose up --build

 * My GUI GUI: http://localhost:12345/
 * My GUI API: http://localhost:12345/swagger/
