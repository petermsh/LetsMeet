import { Component, EventEmitter, Output } from '@angular/core';
import { AuthorizationService } from '../authorization/authorization.service';
import {HubClientService} from '../hub/hub-client.service';

@Component({
  selector: 'app-header',
  standalone: false,
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {

  get isLoggedIn() {
    return this.authorizationService.isLoggedIn;
  }

  @Output() logout = new EventEmitter();

  onLogoutClicked() {
    this.logout.emit();
  }

  async startConnection() {
    await this.hubClientService.startConnection();
  }

  constructor(private authorizationService: AuthorizationService, private hubClientService: HubClientService) {
  }
}
