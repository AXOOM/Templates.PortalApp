import { Component } from '@angular/core';
import { AuthConfig, JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { environment } from 'src/environments/environment';

import { OAuthConfig } from './app.config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  oAuthConfig: OAuthConfig;

  constructor(
    private oAuthService: OAuthService,
  ) {
    this.oAuthConfig = Object.assign({}, (<any>window).AXOOM.CONFIG.OAUTH);
    const config: AuthConfig = {
      // Url of the Identity Provider
      issuer: this.oAuthConfig.identityServerUri,

      // URL of the SPA to redirect the user to after login
      redirectUri: window.location.protocol + '//' + window.location.host,

      // URL of the SPA to redirect the user after silent refresh
      silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',

      postLogoutRedirectUri: window.location.protocol + '//' + window.location.host,

      // The SPA's id. The SPA is registerd with this id at the auth-server
      clientId: this.oAuthConfig.clientId,

      // set the scope for the permissions the client should request
      // The first three are defined by OIDC. The 4th is a usecase-specific one
      scope: this.oAuthConfig.scope,

      // Demands using https as OIDC and OAuth2 relay on it.
      // This rule can be relaxed using the property requireHttps, e. g. for local testing.
      requireHttps: false,

      // Activate Session Checks:
      sessionChecksEnabled: environment.production, // set to false for local deployment to facilitate calls to remote CIS API

      clearHashAfterLogin: false
    };
    this.oAuthService.configure(config);
    this.oAuthService.tokenValidationHandler = new JwksValidationHandler();
  }
}
