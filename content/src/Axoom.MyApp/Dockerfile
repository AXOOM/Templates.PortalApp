FROM microsoft/aspnetcore-build:2.0.5-2.1.4
ARG source
WORKDIR /app
EXPOSE 80
EXPOSE 5000
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Axoom.MyApp.dll"]
