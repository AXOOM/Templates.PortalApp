# My App

my description

## Development

Run `build.ps1` to compile the source code and package the result in Docker images. This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

Configuration overrides for local development are specified in:
- [launchSettings.json](src/App/Properties/launchSettings.json) for IDEs
- [docker-compose.override.yml](src/docker-compose.override.yml) for Docker Compose

To enable authentication against the Identity Server during local development run:
```powershell
cd src\App
dotnet user-secrets set Authentication:Authority https://account.dev.myaxoom.com
dotnet user-secrets set Authentication:ApiSecret thisissecret
```

To allow Prometheus metrics to be exposed when running locally run:
```powershell
netsh http add urlacl http://*:5000/ user=$env:USERDOMAIN\$env:USERNAME
```
You can then access the metrics at: http://localhost:5000/

To build and then run locally with Docker Compose:
```powershell
cd src
./build-dotnet.ps1
docker-compose up --build
```
You can then interact with the App at: http://localhost:12345/
