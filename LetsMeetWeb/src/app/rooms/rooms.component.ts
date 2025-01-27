import {Component, OnDestroy, OnInit} from '@angular/core';
import { RoomsService } from './rooms.service';
import {MessagesService} from '../messages/messages.service';
import {MessageDto} from '../messages/messageDto';
import {HubClientService} from '../hub/hub-client.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-rooms',
  standalone: false,
  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css'
})
export class RoomsComponent implements OnInit {
  rooms: any[] = [];
  selectedRoom: any = null;
  messages: MessageDto[] = [];
  newMessage: string = '';

  constructor(private roomsService: RoomsService,
              private messagesService: MessagesService,
              private hubClientService: HubClientService) {}

  ngOnInit() {
    this.roomsService.getRooms()
      .subscribe({
        next: rooms => {
          console.log("rooms", this.rooms);
          this.rooms = rooms
        }
      });

    this.hubClientService.messageReceived$.subscribe(
      {
        next: message => {
          console.log("mess:", message);
          this.messagesService.getMessages(message.roomId).subscribe({
              next: messages => {
                this.messages = messages;
                console.log('messages: ', this.messages);
              },
              error: err => {
                console.error('Error fetching messages: ', err);
              }
            }
          );
          this.roomsService.getRooms()
            .subscribe({
              next: rooms => {
                console.log("rooms", this.rooms);
                this.rooms = rooms
              }
            });
        }
      }
    );
  }

  selectRoom(room: any) {
    this.selectedRoom = room;
    this.hubClientService.joinRoom(this.selectedRoom);
    console.log("selected: ", this.selectedRoom);
    this.messagesService.getMessages(room.roomId).subscribe({
      next: messages => {
        this.messages = messages;
        console.log('messages: ', this.messages);
      },
      error: err => {
        console.error('Error fetching messages: ', err);
      }
    });
  }

  sendMessage() {
    console.log("selected: ", this.selectedRoom);
    if (this.newMessage.trim() && this.selectedRoom) {
      this.hubClientService.sendMessage(this.selectedRoom.roomId, this.newMessage.trim())
        .subscribe({
          error: err => console.error('Error sending message:', err)
        });
      this.newMessage = '';
    }
  }

  formatRoomDate(date: string): string {
    const now = new Date();
    const messageDate = new Date(date);

    const isToday =
      messageDate.getDate() === now.getDate() &&
      messageDate.getMonth() === now.getMonth() &&
      messageDate.getFullYear() === now.getFullYear();

    const isYesterday =
      messageDate.getDate() === now.getDate() - 1 &&
      messageDate.getMonth() === now.getMonth() &&
      messageDate.getFullYear() === now.getFullYear();

    if (isToday) {
      return messageDate.toLocaleTimeString('pl-PL', {
        hour: '2-digit',
        minute: '2-digit',
      });
    } else if (isYesterday) {
      return 'Wczoraj';
    } else {
      return messageDate.toLocaleDateString('pl-PL', {
        day: '2-digit',
        month: '2-digit',
      });
    }
  }
}
