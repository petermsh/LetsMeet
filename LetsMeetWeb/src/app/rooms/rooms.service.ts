import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/enviroment.development';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  constructor(private httpClient: HttpClient) { }

  getRooms() {
    return this.httpClient.get<Array<any>>(`${environment.apiUrl}/Room`);
  }

  createRoom(roomDetails: any) {
    return this.httpClient.post(`${environment.apiUrl}/Room`, roomDetails);
  }
}
