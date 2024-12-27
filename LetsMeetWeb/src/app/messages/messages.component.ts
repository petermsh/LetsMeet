import { Component } from '@angular/core';
import { AuthorizationService } from '../authorization/authorization.service';

@Component({
  selector: 'app-messages',
  standalone: false,

  templateUrl: './messages.component.html',
  styleUrl: './messages.component.css'
})
export class MessagesComponent {
  title = 'LetsMeet';
  get isLoggedIn() {
    return this.authorizationService.isLoggedIn;
  }

  constructor(private authorizationService: AuthorizationService) {
  }

  onLogout() {
    this.authorizationService.logout();
  }
}
