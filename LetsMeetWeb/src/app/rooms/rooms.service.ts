import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/enviroment.development';

@Injectable({
  providedIn: 'root'
})
export class RoomsService {

  getRooms() {
    return this.httpClient.get<Array<any>>(`${environment.apiUrl}/Room`);
  }

  constructor(private httpClient: HttpClient) { }
}
