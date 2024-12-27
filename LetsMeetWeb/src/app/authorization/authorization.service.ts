import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/enviroment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  logged = false;

  get isLoggedIn() {
    return this.logged;
  }

  login(userName: any, password: any) {
    this.httpClient.post(`${environment.apiUrl}/Auth/login`, {userName: userName, password: password})
      .subscribe({
        next: response => {
          this.logged = true;
          this.router.navigate(['/home']);
        },
        error: error => console.log(error)
      })
  }

  logout() {
    this.logged = false;
    this.router.navigate(['/login']);
  }
  constructor(private router: Router, private httpClient: HttpClient) { }
}
