import { Component } from '@angular/core';
import {AuthorizationService} from '../authorization/authorization.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  userName: any;
  password: any;

  constructor(private authorizationService: AuthorizationService) {
  }

  login() {
    this.authorizationService.login(this.userName, this.password);
  }
}
