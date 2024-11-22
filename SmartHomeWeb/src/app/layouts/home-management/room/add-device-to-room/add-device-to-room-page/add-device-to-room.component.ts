import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import {RoomService} from '../../../../../../backend/services/home-management/room/room.service';
import HardwareIdModel from '../../../../../../backend/services/home-management/room/models/AddDeviceToRoomModel';

@Component({
  selector: 'app-add-device-to-room',
  templateUrl: './add-device-to-room.component.html',
  styleUrl: './add-device-to-room.component.css',
  providers: [],
})
export class AddDeviceToRoomComponent {
  homeHasSelected: boolean = false;
  formElements = [
    {name: 'deviceId', type: 'select-with-values', label: 'Device', values: [], options: []},
    {name: 'roomId', type: 'select-with-values', label: 'Room', values: [], options: []},
  ];
  messageHome: string = '';
  message: string = '';
  isHomeSuccess: boolean = false;
  isSuccess: boolean = false;


  constructor(private homeOwnerService: HomeOwnerService, private roomService: RoomService) {
  }


  onHomeSubmit(formData: any) {
    this.homeOwnerService.getHomeDevices(formData.homeId, '').subscribe((response: any) => {
      this.formElements[0].options = response.map((device: any) => device.name ?? '-');
      this.formElements[0].values = response.map((device: any) => device.homeDeviceId);
      this.homeHasSelected = true;
      this.messageHome = '';
      this.isHomeSuccess = true;
    }, (error: any) => {
      this.isHomeSuccess = false;
      this.messageHome = error.details;
    });

    this.roomService.getRooms(formData.homeId).subscribe((response: any) => {
      this.formElements[1].options = response.map((room: any) => room.name);
      this.formElements[1].values = response.map((room: any) => room.id);
    }, (error: any) => {
    });
  }

  onFormSubmit(formData: any) {
    let model: HardwareIdModel = {
      HardwareId: formData.deviceId, // Ensure this is the correct field
    }
    this.roomService.addDeviceToRoom(formData.roomId, model).subscribe(
      (response: any) => {
        this.message = response;
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }
}
