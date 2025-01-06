import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalR';
import {Observable, Subject} from 'rxjs';
import {HubConnection, HubConnectionBuilder, IHttpConnectionOptions} from '@microsoft/signalR';

@Injectable({
  providedIn: 'root'
})
export class HubClientService {

  private options: IHttpConnectionOptions = {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
  };
  private hubConnection: signalR.HubConnection | null = null;
  private messageSubject = new Subject<any>();

  constructor() {
  }

  public startConnection() {
    console.log("connect");
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5086/chat?access_token=' + this.getAccessToken(), this.options)
      .configureLogging(signalR.LogLevel.Debug)
      .build();

    this.registerReceiveMessageHandler();

    this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          this.registerReceiveMessageHandler();
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
          console.log(`Joined room: ${roomId}`);
        })
        .catch(err => console.error(err));
    }
  }

  leaveRoom(roomId: string): Observable<void> {
    return new Observable<void>((observer) => {
      if(this.hubConnection != null) {
        this.hubConnection.invoke('LeaveRoom', roomId)
          .then(() => {
            console.log(`Left room: ${roomId}`);
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

  public receiveMessage(): Observable<any> {
    return this.messageSubject.asObservable();
  }

  private registerReceiveMessageHandler(): void {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveMessage', (message) => {
        console.log('Message received:', message);
        this.messageSubject.next(message);
      });
    } else {
      console.error('HubConnection not initialized');
    }
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
            console.log("token: ", parsedUser.accessToken);
            return parsedUser.accessToken || '';
          } catch (error) {
            console.error("Failed to parse 'currentUser':", error);
          }
        }
      }
    return 'error';
  }
}


