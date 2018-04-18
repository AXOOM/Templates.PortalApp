import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html'
})
export class ContactsComponent {
  contacts: ContactDto[];

  constructor(
    http: HttpClient,
    private oauthService: OAuthService,
    @Inject('BASE_URL') baseUrl: string
  ) {
    const getOptions = {
      headers: this.getHeaders()
    };

    http.get<ContactDto[]>(baseUrl + 'api/contacts/', getOptions).subscribe(result => {
        this.contacts = result;
      },
      error => console.error(error));
  }

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Authorization': `Bearer ${this.oauthService.getAccessToken()}`
    });
  }
}

interface ContactDto {
  id: string;
  firstName: string;
  lastName: string;
}

interface NoteDto {
  content: string;
}
