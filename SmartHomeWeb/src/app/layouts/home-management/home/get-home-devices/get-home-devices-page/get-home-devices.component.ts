import {Component} from '@angular/core';
import {HomeOwnerService} from "../../../../../../backend/services/home-management/home-owner/home-owner.service";
import {RoomService} from '../../../../../../backend/services/home-management/room/room.service';

@Component({
  selector: 'app-get-home-devices',
  templateUrl: './get-home-devices.component.html',
  styleUrl: './get-home-devices.component.css',
  providers: [HomeOwnerService],
})
export class GetHomeDevicesComponent {
  roomElements = [
    {name: 'roomId', type: 'select-with-values', label: 'Room', values: [] as any[], options: [] as any[]},
  ];
  hasSelectedHome = false;
  hasSelectedRoom = false;
  homeId: string = '';
  devices: any = [];
  messageHome: string = '';
  message: string = '';
  isHomeSuccess: boolean = false;
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService, private roomService: RoomService) {
  }

  onHomeSubmit(formData: any) {
    this.homeId = formData.homeId;
    this.loadRooms(formData.homeId);
  }

  onRoomSubmit(formData: any) {
    this.hasSelectedRoom = true;
    this.homeOwnerService.getHomeDevices(this.homeId, formData.roomId).subscribe((response: any) => {
      this.devices = response;
      this.message = '';
      this.isSuccess = true;
    }, (error: any) => {
      console.error('Error:', error);
      this.message = error.details;
      this.isSuccess = false
    });
  }

  loadRooms(homeId: string
  ) {
    this.roomService.getRooms(homeId).subscribe((rooms: any) => {
        this.roomElements[0].values = rooms.map((room: any) => room.id);
        this.roomElements[0].options = rooms.map((room: any) => room.name);
        this.roomElements[0].options.push('All');
        this.roomElements[0].values.push('');
        this.hasSelectedHome = true;
        this.hasSelectedRoom = false;
        this.messageHome = '';
        this.isHomeSuccess = true;
      }, (error: any) => {
        console.error('Error:', error);
        this.messageHome = error.details;
        this.isHomeSuccess = false;
      }
    );
  }
}
