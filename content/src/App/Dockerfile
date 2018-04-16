FROM microsoft/aspnetcore-build:2.0.5-2.1.4
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
CMD if [ -n "$IDENTITY_SERVER_URI" ]; \
      then echo "AXOOM_APP.CONFIG.OAUTH.identityServerUri = \"$IDENTITY_SERVER_URI\";" >> ./ClientApp/dist/assets/config.js; \
    fi; \
    env; \
	dotnet Axoom.MyApp.dll
