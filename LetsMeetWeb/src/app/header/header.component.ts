import { Component, EventEmitter, Output } from '@angular/core';
import { AuthorizationService } from '../authorization/authorization.service';

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

  constructor(private authorizationService: AuthorizationService) {
  }
}
