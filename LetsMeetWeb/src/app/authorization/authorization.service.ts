import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/enviroment.development';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private logged = false;

  get isLoggedIn() {
    return this.logged;
  }

  get currentUser(): any {
    if (this.isLocalStorageAvailable()) {
      const user = localStorage.getItem('currentUser');
      return user ? JSON.parse(user) : null;
    }
    return null;
  }

  login(userName: any, password: any) {
    this.httpClient.post(`${environment.apiUrl}/Auth/login`, { userName, password })
      .subscribe({
        next: response => {
          if (this.isLocalStorageAvailable()) {
            localStorage.setItem('currentUser', JSON.stringify(response));
          }
          this.logged = true;
          this.router.navigate(['/home']);
        },
        error: error => console.error(error)
      });
  }

  logout() {
    this.logged = false;
    if (this.isLocalStorageAvailable()) {
      localStorage.removeItem('currentUser');
    }
    this.router.navigate(['/login']);
  }

  private isLocalStorageAvailable(): boolean {
    try {
      return typeof window !== 'undefined' && 'localStorage' in window && window.localStorage !== null;
    } catch (error) {
      console.error('localStorage is not available:', error);
      return false;
    }
  }

  constructor(private router: Router, private httpClient: HttpClient) {
    if (this.isLocalStorageAvailable()) {
      const user = localStorage.getItem('currentUser');
      if (user) {
        this.logged = true;
      }
    }
  }
}