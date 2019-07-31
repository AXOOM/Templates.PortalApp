import { Injectable } from '@angular/core';
import { JsonConvert, JsonObject, JsonProperty } from 'json2typescript';

@JsonObject
export class OAuthConfig {

  @JsonProperty('clientId')
  clientId = '';

  @JsonProperty('scope')
  scope = '';

  @JsonProperty('identityServerUri')
  identityServerUri = '';
}

@JsonObject
export class AppConfig {

  @JsonProperty('OAUTH')
  oAuthConfig: OAuthConfig = new OAuthConfig();

}

@Injectable()
export class AppConfigService {

  private static COOKIE_KEY_FOR_LANGUAGE = 'language';

  public config: AppConfig;

  constructor() {
    const jsonConvert: JsonConvert = new JsonConvert();
    jsonConvert.ignorePrimitiveChecks = false; // don't allow assigning number to string etc.
    this.config = <AppConfig>(jsonConvert.deserialize((<any>window).AXOOM.CONFIG, AppConfig));
  }

  public setLanguage(languageKey: string) {
    const oneYearInSeconds = 60 * 60 * 24 * 365;
    // nginx will use this cookie, see file nginx.conf
    document.cookie = AppConfigService.COOKIE_KEY_FOR_LANGUAGE + '=' + languageKey + ';path=/;max-age=' + oneYearInSeconds;
  }

}
