# My App

This repository contains the source code for My App.

## Development

Run `build.ps1` to compile the source code and package the result in Docker images. This script takes a version number as an input argument. The source code itself contains no version numbers. Instead version numbers should be determined at build time using [GitVersion](http://gitversion.readthedocs.io/).

To build and run locally with Docker Compose:

    cd src
    ./build-dotnet.ps1
    docker-compose up --build

### Add Authentication and Authorization
#### Backend
Boilerplate code for web api authorization is already added. However, you must pass IDENTITY_SERVER_URI to activate it.
Just uncomment the line with IDENTITY_SERVER_URI in appsettings.yml to activate it. For docker mode, you can also 
uncomment the same in docker-compose.override.yml.

Note: Authorization is added to all controllers by default.
```csharp
    services
        .AddMvc(options =>
        {
            options.Filters.Add(typeof(ApiExceptionFilterAttribute));
            if (identityServerUri != null)
                options.Filters.Add(new AuthorizeFilter(ScopePolicy.Create(apiName)));
        });
```

#### Frontend
Boilerplate code for authentication and/or authorization is already added. However, you must pass identityServerUri to activate it.
Just uncomment the line with identityServerUri in assets/config.js to activate it. 

```javascript
AXOOM_APP.CONFIG = {
    OAUTH: {
        clientId: 'myvendor-myapp',
        scope: 'openid profile email myvendor-myapp.api',
        // Uncomment below line to activate Authentication in frontend.
        //identityServerUri: 'http://identity-myinstance.local.myaxoom.eu'
    }
};
```
Adjust clientId/scope/identityServerUri accordingly.

**Protect Route Access**

By using canActivate property and AuthGuard class, you can protect your route. Please check app.module.ts for sample.

```typescript
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'contacts', component: ContactsComponent, canActivate: [AuthGuard] },
    ])
```

For more information on route protection, please refer to [router and navigation documentation](https://angular.io/guide/router).

We are using angular-oauth2-oidc library for oauth. Please refer to [official documentation](https://github.com/manfredsteyer/angular-oauth2-oidc) for more information.

**Pass access_token for Api access**

You can use below authorization header to pass access_token for web api access.

```typescript
  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': 'Bearer ' + this.oauthService.getAccessToken()
    });
  }

```

## Deploying

### Feed URI

http://assets.axoom.cloud/apps/myvendor-myapp.xml

### External environment

| Name | Default | Description |
| ---- | ------- | ----------- |
|      |         |             |


