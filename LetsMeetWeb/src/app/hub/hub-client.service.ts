import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalR';
import {Observable, Subject} from 'rxjs';
import {HubConnection, HubConnectionBuilder, IHttpConnectionOptions} from '@microsoft/signalR';
import {MessageDto} from '../messages/messageDto';

@Injectable({
  providedIn: 'root'
})
export class HubClientService {

  private options: IHttpConnectionOptions = {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  };
  private hubConnection: signalR.HubConnection | null = null;

  private messageReceivedSubject = new Subject<MessageDto>();
  public messageReceived$ = this.messageReceivedSubject.asObservable();

  constructor() {
  }

  public startConnection() {
    console.log("connect");
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5086/chat?access_token=' + this.getAccessToken(), this.options)
      .configureLogging(signalR.LogLevel.Debug)
      .build();

    this.hubConnection.on('ReceiveMessage', (message: MessageDto) => {
      this.messageReceivedSubject.next(message);
    });

    this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
        });
  }

  public async joinRoom(roomId: string) {
    if(this.hubConnection != null)
    {
      await this.hubConnection.invoke('JoinRoom', roomId)
        .then(() => {
        })
        .catch(err => console.error(err));
    }
  }

  leaveRoom(roomId: string): Observable<void> {
    return new Observable<void>((observer) => {
      if(this.hubConnection != null) {
        this.hubConnection.invoke('LeaveRoom', roomId)
          .then(() => {
            observer.complete();
          })
          .catch(err => observer.error(err));
      }
    });
  }

  sendMessage(roomId: string, content: string): Observable<void> {
    return new Observable<void>((observer) => {
      if(this.hubConnection != null) {
        this.hubConnection.invoke('SendMessage', {roomId, content})
          .then(() => observer.complete())
          .catch(err => observer.error(err));
      }
    });
  }

  private isLocalStorageAvailable(): boolean {
    try {
      return typeof window !== 'undefined' && 'localStorage' in window && window.localStorage !== null;
    } catch (error) {
      console.error('localStorage is not available:', error);
      return false;
    }
  }

  private getAccessToken() : string {
      if (this.isLocalStorageAvailable()) {
        const storedUser = localStorage.getItem('currentUser');
        if (storedUser) {
          try {
            const parsedUser = JSON.parse(storedUser);
            return parsedUser.accessToken || '';
          } catch (error) {
            console.error("Failed to parse 'currentUser':", error);
          }
        }
      }
    return 'error';
  }

  public getConnectionId(): Observable<string> {
    return new Observable<string>((observer) => {
      if (this.hubConnection != null) {
        this.hubConnection.invoke<string>('GetConnectionId')
          .then((connectionId) => {
            observer.next(connectionId);
            observer.complete();
          })
          .catch((error) => {
            console.error('Error fetching connectionId:', error);
            observer.error(error);
          });
      } else {
        const error = new Error('Hub connection is not initialized.');
        console.error(error.message);
        observer.error(error);
      }
    });
  }

}


