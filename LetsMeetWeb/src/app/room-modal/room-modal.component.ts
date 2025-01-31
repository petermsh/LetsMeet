import {Component, EventEmitter, Output} from '@angular/core';
import {HubClientService} from '../hub/hub-client.service';

@Component({
  selector: 'app-room-modal',
  standalone: false,

  templateUrl: './room-modal.component.html',
  styleUrl: './room-modal.component.css'
})
export class RoomModalComponent {
  roomDetails = {
    connectionId: '',
    university: '',
    city: '',
    major: '',
    gender: 0
  };

  constructor(private hubClientService: HubClientService) {
  }

  @Output() confirm = new EventEmitter<any>();
  @Output() close = new EventEmitter<void>();

  confirmRoom() {
    this.hubClientService.getConnectionId().subscribe({
      next: (connectionId) => {
        this.roomDetails.connectionId = connectionId;
        this.confirm.emit(this.roomDetails);
      },
      error: (err) => {
        console.error('Error retrieving connection ID:', err);
      }
    });
  }

  closeModal() {
    this.close.emit();
  }
}
