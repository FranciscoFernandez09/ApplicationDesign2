import {Component, OnInit} from '@angular/core';
import AddRoomModel from '../../../../../../backend/services/home-management/room/models/AddRoomModel';
import {RoomService} from '../../../../../../backend/services/home-management/room/room.service';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';

@Component({
  selector: 'app-add-room',
  templateUrl: './add-room.component.html',
  styleUrls: ['./add-room.component.css'],
})
export class AddRoomComponent implements OnInit {
  formElements = [
    {name: 'homeId', type: 'select-with-values', label: 'Home', options: [], values: []},
    {name: 'name', type: 'text', label: 'Name'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private roomService: RoomService, private homeOwnerService: HomeOwnerService) {
  }

  ngOnInit() {
    this.loadHomes();
  }

  onFormSubmit(formData: any) {
    let model: AddRoomModel = {
      Name: formData.name,
    }
    this.roomService.addRoom(formData.homeId, model).subscribe(
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

  loadHomes() {
    this.homeOwnerService.getMineHomes().subscribe(
      (response: any) => {
        this.formElements[0].options = response.map((home: any) => home.name);
        this.formElements[0].values = response.map((home: any) => home.homeId);
      },
      (error: any) => {
        console.error('Error loading homes:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}
