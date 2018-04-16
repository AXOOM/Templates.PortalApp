import { Component } from '@angular/core';
import { AuthConfig, JwksValidationHandler, OAuthService } from 'angular-oauth2-oidc';
import { OAuthEvent } from 'angular-oauth2-oidc/events';

import { AppConfigService, OAuthConfig } from './app-config';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  private oAuthConfig: OAuthService;
  constructor(
    private oauthService: OAuthService,
    private configService: AppConfigService
  ) {
    this.configureAuthenticationService();
  }

  private configureAuthenticationService() {

    const authConfig = this.configService.getAuthConfig();
    if (authConfig.issuer) {
      console.log('Authentication is activated. User will be redirected to the IdentityServer for login.');
    } else {
      console.log(`Authentication is not activated. Add identityServerUri in '/assets/config.js' file.`);
      return;
    }

    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.events.subscribe((event: OAuthEvent) => {
      switch (event.type) {
        case 'token_received':
          break;
        case 'token_error':
          break;
        case 'session_terminated':
          this.oauthService.logOut();
          break;
        case 'session_changed':
          break;
      }
    });
    this.oauthService.setupAutomaticSilentRefresh();
  }
}
