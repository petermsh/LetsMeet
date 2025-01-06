import { Component } from '@angular/core';
import {AuthorizationService} from '../authorization/authorization.service';
import {HubClientService} from '../hub/hub-client.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  userName: any;
  password: any;

  constructor(private authorizationService: AuthorizationService, private hubClientService: HubClientService) {
  }

  async login() {
    await this.authorizationService.login(this.userName, this.password);
  }
}
