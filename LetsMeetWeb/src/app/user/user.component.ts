import { Component, OnInit } from '@angular/core';
import { UsersService, User, UpdateUserDto } from './users-service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-profile',
  standalone: false,
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  user: User | null = null;
  errorMessage = '';

  constructor(private usersService: UsersService, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.loadUserData();
  }

  loadUserData() {
    this.usersService.getUser().subscribe({
      next: (data: any) => {
          this.user = data;
      },
      error: (error) => {
        this.errorMessage = 'Błąd podczas pobierania danych użytkownika.';
        console.error(error);
      }
    });
  }

  saveProfile() {
    if (!this.user) return;

    const updateData: UpdateUserDto = {
      bio: this.user.bio,
      city: this.user.city,
      university: this.user.university,
      major: this.user.major
    };

    this.usersService.updateUser(updateData).subscribe({
      next: () => {
        this.snackBar.open('Dane zostały zaktualizowane!', 'Zamknij', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
      },
      error: (error) => {
        this.errorMessage = 'Wystąpił błąd podczas zapisywania danych.';
        this.snackBar.open('Wystąpił błąd podczas zapisywania danych.', 'Zamknij', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
      }
    });
  }
}
