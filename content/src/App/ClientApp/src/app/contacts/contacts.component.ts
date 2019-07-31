import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-contacts',
  templateUrl: './contacts.component.html'
})
export class ContactsComponent {
  contacts: ContactDto[];

  constructor(
    http: HttpClient
  ) {
    http.get<ContactDto[]>('/api/contacts/').subscribe(result => {
        this.contacts = result;
      },
      error => console.error(error));
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
