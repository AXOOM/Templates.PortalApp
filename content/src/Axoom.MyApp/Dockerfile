FROM microsoft/aspnetcore:2.0.4
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "Axoom.MyApp.dll"]
