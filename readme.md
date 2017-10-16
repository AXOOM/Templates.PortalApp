# AXOOM Portal Apps Template

This repository contains a dotnet-new template for AXOOM Portal Apps using ASP.NET Core and Docker.

Run `build.ps1` to package the template as a NuGet package.
This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](gitversion.readthedocs.io).

To install the template run `dotnet new --install axoom-portal-app::*`.
To use the template run `dotnet new axoom-portal-app --appKey axoom-myapp --friendlyName "My App" --description "my description"`.
