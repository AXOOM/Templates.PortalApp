import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpHeaders } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';

/** Add Bearer token to HTTP requests. */
@Injectable()
export class OAuthInterceptor implements HttpInterceptor {

  constructor(
    private oAuthService: OAuthService
  ) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    console.warn('HELLO');
    if (this.oAuthService.issuer) {
      req = req.clone({
        headers: req.headers.set('Authorization', this.oAuthService.authorizationHeader())
      });
    }

    return next.handle(req);
  }
}
