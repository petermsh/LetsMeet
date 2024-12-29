import {Component, OnInit} from '@angular/core';
import { RoomsService } from './rooms.service';

@Component({
  selector: 'app-rooms',
  standalone: false,

  templateUrl: './rooms.component.html',
  styleUrl: './rooms.component.css'
})
export class RoomsComponent implements OnInit {

  rooms: any[] = [];

  constructor(private roomsService: RoomsService) {}

  ngOnInit() {
    this.roomsService.getRooms()
      .subscribe({
        next: rooms => {
          console.log("rooms", this.rooms);
          this.rooms = rooms
        }
      });
  }
}
