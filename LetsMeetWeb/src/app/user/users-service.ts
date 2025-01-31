import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/enviroment.development';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private httpClient: HttpClient) { }

  getUserByName(userName: string) {
    return this.httpClient.get(`${environment.apiUrl}/User/${userName}`);
  }

  getUser() {
    return this.httpClient.get(`${environment.apiUrl}/User`);
  }

  updateUser(userDto: UpdateUserDto) {
    return this.httpClient.patch(`${environment.apiUrl}/User/info`, userDto);
  }
}

export interface User {
  id: string;
  userName: string;
  age: number;
  gender: string;
  bio: string;
  city: string;
  university: string;
  major: string;
}

export interface UpdateUserDto {
  bio?: string;
  city?: string;
  university?: string;
  major?: string;
}
