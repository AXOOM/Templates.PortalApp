import { JsonConvert, OperationMode, ValueCheckingMode } from 'json2typescript';
import { JsonObject, JsonProperty } from 'json2typescript';
import { Injectable } from '@angular/core';
import { AuthConfig } from 'angular-oauth2-oidc';

@JsonObject
export class AppConfig {
    @JsonProperty('OAUTH')
    oAuthConfig: OAuthConfig = new OAuthConfig();
}

@JsonObject
export class OAuthConfig {
    @JsonProperty('clientId')
    clientId = '';
    @JsonProperty('scope')
    scope = '';
    @JsonProperty('identityServerUri')
    identityServerUri = '';
}


@Injectable()
export class AppConfigService {

    public config: AppConfig;
    constructor() {
        const jsonConvert: JsonConvert = new JsonConvert();
        jsonConvert.ignorePrimitiveChecks = false; // don't allow assigning number to string etc.
        this.config = jsonConvert.deserialize((<any>window).AXOOM_APP.CONFIG, AppConfig);
    }

    public getAuthConfig(): AuthConfig {

        const oAuthConfig = this.config.oAuthConfig;

        const authConfig: AuthConfig = {
            // Url of the Identity Provider
            issuer: oAuthConfig.identityServerUri,

            // URL of the SPA to redirect the user to after login
            redirectUri: window.location.protocol + '//' + window.location.host,

            // URL of the SPA to redirect the user after silent refresh
            silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',

            postLogoutRedirectUri: window.location.protocol + '//' + window.location.host,

            // The SPA's id. The SPA is registerd with this id at the auth-server
            clientId: oAuthConfig.clientId,

            // set the scope for the permissions the client should request
            // The first three are defined by OIDC. The 4th is a usecase-specific one
            scope: oAuthConfig.scope,

            // Demands using https as OIDC and OAuth2 relay on it.
            // This rule can be relaxed using the property requireHttps, e. g. for local testing.
            requireHttps: false,

            // Activate Session Checks:
            sessionChecksEnabled: true
        };

        return authConfig;
    }
}
