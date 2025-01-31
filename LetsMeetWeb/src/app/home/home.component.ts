import { Component } from '@angular/core';
import {AuthorizationService} from '../authorization/authorization.service';
import {HubClientService} from '../hub/hub-client.service';

@Component({
  selector: 'app-home',
  standalone: false,

  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  constructor(private hubClientService: HubClientService) {
    this.hubClientService.startConnection();
  }

}
