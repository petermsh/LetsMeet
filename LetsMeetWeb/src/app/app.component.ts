import {Component, EventEmitter} from '@angular/core';
import { AuthorizationService } from './authorization/authorization.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  isLoginView: boolean = true;
  toggleViewEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  get isLoggedIn() {
    return this.authorizationService.isLoggedIn;
  }

  onLogout() {
    this.authorizationService.logout();
  }

  constructor(private authorizationService: AuthorizationService) {
    this.toggleViewEvent.subscribe(value => this.isLoginView = value);
  }

  toggleView(value: boolean) {
    this.isLoginView = value;
  }
}
