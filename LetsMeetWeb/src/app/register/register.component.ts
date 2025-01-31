import {Component, EventEmitter, Output} from '@angular/core';
import {AuthorizationService, RegisterDto} from '../authorization/authorization.service';
import {Router} from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  standalone: false,

  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {

  @Output() toggleViewEvent = new EventEmitter<boolean>();

  registerDto: RegisterDto = {
    userName: '',
    password: '',
    age: 0,
    gender: 0,
    city: '',
    university: '',
    major: ''
  };
  validationErrors: any = {};

  constructor(private authorizationService: AuthorizationService,
              private snackBar: MatSnackBar) {
  }

  async register() {
    try {
      await this.authorizationService.register(this.registerDto);
      this.showPopup('Registration Successful', 'success');
      this.toggleViewEvent.emit(true);
    } catch (error: any) {
      if (error.status === 400 && error.error?.errors) {
        this.validationErrors = error.error.errors;
      } else {
        this.validationErrors = {};
      }
      const errorMessage = error.error?.message || 'An error occurred during registration. Please try again.';
      this.showPopup(errorMessage, 'error');
    }
  }

  showPopup(message: string, type: 'success' | 'error'): void {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: type
    });
  }
}
