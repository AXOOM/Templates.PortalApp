FROM microsoft/aspnetcore-build:2.0.8-2.1.200
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "MyVendor.MyApp.dll"]
