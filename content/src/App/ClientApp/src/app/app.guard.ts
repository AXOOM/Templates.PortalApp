import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, CanLoad, Route } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { OAuthEvent } from 'angular-oauth2-oidc/events';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(
    private oauthService: OAuthService
  ) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const url: string = state.url;
    return this.checkLogin(url, state);
  }

  private checkLogin(url: string, routeState: RouterStateSnapshot): boolean | Promise<boolean> {

    if (this.oauthService.hasValidIdToken() && this.oauthService.hasValidAccessToken()) { return true; }

    return this.oauthService.loadDiscoveryDocumentAndTryLogin().then(() => {

      if (this.oauthService.hasValidIdToken() && this.oauthService.hasValidAccessToken()) {

        this.oauthService.loadUserProfile().then(o => {
          const profile = (o as UserProfile);
          console.log('logged-in-user: ' + profile.preferred_username);
        });

        return true;

      } else {
        this.oauthService.initImplicitFlow(url.replace(';', '~semicolon~'));
        return true;
      }
    }).catch((reason) => {
      console.error(reason);
      this.oauthService.initImplicitFlow(url.replace(';', '~semicolon~'));
      return true;
    });
  }

}
export interface UserProfile {
  sid: string;
  email?: string;
  sub: string;
  given_name?: string;
  family_name?: string;
  preferred_username?: string;
  picture?: string;
  locale?: string;
}
