import {Component, OnInit} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import AddDeviceModel from '../../../../../../backend/services/home-management/home-owner/models/AddDeviceModel';
import {UserService} from "../../../../../../backend/services/user/user.service";
import FilterDeviceModel from "../../../../../../backend/services/user/models/FilterDeviceModel";

@Component({
  selector: 'app-add-device',
  templateUrl: './add-device.component.html',
  styleUrl: './add-device.component.css',
  providers: [HomeOwnerService, UserService],
})
export class AddDeviceComponent implements OnInit {

  limit: number = 10;
  offset: number = 0;
  name: string = '';

  homeId: string = '';

  devices: any[] = [];

  message: string = '';
  isSuccess: boolean = false;


  filterElements: any[] = [
    {
      type: 'text',
      name: 'name',
      label: 'Name',
    },];

  constructor(private homeOwnerService: HomeOwnerService, private userService: UserService) {
  }

  ngOnInit() {
    this.loadDevices();
  }

  onHomeSubmit(formData: any) {
    if (!formData.homeId) {
      this.message = 'Please select a home';
      this.isSuccess = false;
      return;
    }
    this.homeId = formData.homeId ?? '';
    this.message = 'Selected home successfully';
    this.isSuccess = true;
  }

  onPageChange(event: any) {
    this.offset = event.offset ?? 0;
    this.limit = event.limit ?? 10;
    this.name = event.formData.name ?? '';
    this.loadDevices();
  }

  loadDevices() {
    let model: FilterDeviceModel = {
      name: this.name,
      model: '',
      companyName: '',
      type: '',
      offset: this.offset,
      limit: this.limit,
    }
    this.userService.getDevices(model).subscribe(
      (response: any) => {
        console.log('Devices:', response);
        this.devices = response;
      },
      (error: any) => {
        console.error('Error loading devices:', error);
      });
  }

  addDevice(deviceId: string) {
    let model: AddDeviceModel = {
      DeviceId: deviceId,
    }
    this.homeOwnerService.addDevice(model, this.homeId).subscribe(
      (response: any) => {
        console.log('Add device response:', response);
        this.message = 'Device added successfully';
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error adding device:', error);
        this.message = 'Error adding device';
        this.isSuccess = false;
      });
  }
}
