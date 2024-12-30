import {Component, OnInit} from '@angular/core';
import { RoomsService } from './rooms.service';
import {MessagesService} from '../messages/messages.service';
import {MessageDto} from '../messages/messageDto';

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

  constructor(private roomsService: RoomsService, private messagesService: MessagesService) {}

  ngOnInit() {
    this.roomsService.getRooms()
      .subscribe({
        next: rooms => {
          console.log("rooms", this.rooms);
          this.rooms = rooms
        }
      });
  }
  selectRoom(room: any) {
    this.selectedRoom = room;
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
}
