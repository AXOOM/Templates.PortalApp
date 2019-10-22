FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100

# Add Node.js
ARG NODE_VERSION=10.15.3
RUN curl --silent --location "https://nodejs.org/dist/v${NODE_VERSION}/node-v${NODE_VERSION}-linux-x64.tar.gz" | tar -xz -C /usr/local --strip-components=1
RUN ln -s /usr/local/bin/node /usr/local/bin/nodejs

ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MyVendor.MyApp.dll"]
