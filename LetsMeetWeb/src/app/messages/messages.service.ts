import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environments/enviroment.development';
import {MessageDto} from './messageDto';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {

  getMessages(roomId: string): Observable<MessageDto[]> {
    return this.httpClient.get<MessageDto[]>(`${environment.apiUrl}/Messages`, {
      params: { roomId }
    });
  }

  constructor(private httpClient: HttpClient) { }
}
