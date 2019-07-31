/* tslint:disable: no-console*/
import { Inject, Injectable, LOCALE_ID } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { OAuthEvent } from 'angular-oauth2-oidc/events';

import { AppConfigService } from './app.config';


@Injectable()
export class AuthGuard implements CanActivate {

  private currentLocale: string;

  private static stateFromUrl(url): string {
    return encodeURIComponent(url);
  }

  private static urlFromState(state): string {
    return state;
  }

  constructor(
    private oAuthService: OAuthService,
    private router: Router,
    private configService: AppConfigService,
    @Inject(LOCALE_ID) currentLocale: string
  ) {
    console.info('Portal current locale: ', currentLocale);
    this.currentLocale = currentLocale;
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (!this.oAuthService.issuer) {
      return true; // Authentication is disabled
    }

    const url: string = state.url;
    return this.checkLogin(url, state);
  }

  private configOAuthService(oAuthService: OAuthService) {
    oAuthService.events.subscribe((event: OAuthEvent) => {
      switch (event.type) {
        case 'token_received':
          break;
        case 'token_error':
          break;
        case 'session_terminated':
          oAuthService.logOut();
          break;
        case 'session_changed':
          break;
      }
    });
    oAuthService.setupAutomaticSilentRefresh();
  }

  private checkLogin(url: string, routeState: RouterStateSnapshot): boolean | Promise<boolean> {
    // TODO: Evaluate new feature in OpenID Library this.oAuthService.loadDiscoveryDocumentAndLogin()
    if (this.oAuthService.hasValidAccessToken()) { return true; }

    return this.oAuthService.loadDiscoveryDocumentAndTryLogin().then(() => {
        if (this.oAuthService.hasValidAccessToken()) {

          this.configOAuthService(this.oAuthService);

          this.router.navigateByUrl(AuthGuard.urlFromState(this.oAuthService.state));

          return true;
        } else {
          const state = AuthGuard.stateFromUrl(url);
          this.oAuthService.initImplicitFlow(state);
        }
      }).catch(error => {
        if (error.type === 'invalid_nonce_in_state') {
          // If nonce/state was wrong/missing bounce to identity server once more.
          console.warn('The state/nonce was not available, might have accessed the login page from an bookmark. Reinitiating the login.');
          const state = AuthGuard.stateFromUrl(this.oAuthService.state);
          this.oAuthService.initImplicitFlow(state);
        } else {
          console.warn('Unable to access identity server.');
          return false;
        }
      });
  }

}
