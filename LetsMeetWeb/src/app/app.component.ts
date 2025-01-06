import { Component } from '@angular/core';
import { AuthorizationService } from './authorization/authorization.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {

  get isLoggedIn() {
    return this.authorizationService.isLoggedIn;
  }

  onLogout() {
    this.authorizationService.logout();
  }

  constructor(private authorizationService: AuthorizationService) {
  }
}
