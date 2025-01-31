import {Component, OnDestroy, OnInit} from '@angular/core';
import { RoomsService } from './rooms.service';
import {MessagesService} from '../messages/messages.service';
import {MessageDto} from '../messages/messageDto';
import {HubClientService} from '../hub/hub-client.service';
import {UsersService} from '../user/users-service';
import { MatSnackBar } from '@angular/material/snack-bar';

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
  selectedUser: any = null;
  newMessage: string = '';

  constructor(private roomsService: RoomsService,
              private messagesService: MessagesService,
              private hubClientService: HubClientService,
              private usersService: UsersService,
              private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.getRooms();

    this.hubClientService.messageReceived$.subscribe(
      {
        next: message => {
          this.messagesService.getMessages(message.roomId).subscribe({
              next: messages => {
                this.messages = messages;
              },
              error: err => {
                console.error('Error fetching messages: ', err);
              }
            }
          );
          this.getRooms();
        }
      }
    );
  }

  getRooms() {
    this.roomsService.getRooms()
      .subscribe({
        next: rooms => {
          this.rooms = rooms
        }
      });
  }

  selectRoom(room: any) {
    this.selectedRoom = room;
    this.hubClientService.joinRoom(this.selectedRoom);
    this.messagesService.getMessages(room.roomId).subscribe({
      next: messages => {
        this.messages = messages;
      },
      error: err => {
        console.error('Error fetching messages: ', err);
      }
    });
    this.getUserByName(room.roomName);
  }

  sendMessage() {
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

  formatMessageDate(date: string): string {
    const messageDate = new Date(date);

    return messageDate.toLocaleDateString('pl-PL', {
        day: '2-digit',
        month: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
      });
  }

  isModalOpen: boolean = false;

  isServerMessage(message: MessageDto): boolean
  {
    return message.from === "Server";
  }

  openModal() {
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  handleConfirmRoom(roomDetails: any) {
    this.roomsService.createRoom(roomDetails).subscribe({
      next: () => {
        this.snackBar.open('Pokój został pomyślnie utworzony!', 'Zamknij', {
          duration: 3000,
          panelClass: ['success-snackbar']
        });
        this.getRooms();
        this.closeModal();
      },
      error: (err) => {
        console.error('Błąd podczas tworzenia pokoju:', err);
        this.snackBar.open('Nie udało się utworzyć pokoju.', 'Zamknij', {
          duration: 3000,
          panelClass: ['error-snackbar']
        });
        this.closeModal();
      },
    });
  }

  getUserByName(userName: string) {
    this.usersService.getUserByName(userName).subscribe({
      next: user => this.selectedUser = user,
      error: err => console.error('Error fetching user info:', err)
    });
  }

}
