import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import {HomeDeviceService} from '../../../../../../backend/services/home-management/home-device/home-device.service';
import ModifyHomeDeviceNameModel
  from '../../../../../../backend/services/home-management/home-device/models/ModifyHomeDeviceNameModel';

@Component({
  selector: 'app-modify-home-device-name',
  templateUrl: './modify-home-device-name.component.html',
  styleUrls: ['./modify-home-device-name.component.css'],
})
export class ModifyHomeDeviceNameComponent {
  deviceFormElements = [
    {name: 'hardwareId', type: 'select-with-values', label: 'Device', values: [], options: []},
    {name: 'name', type: 'text', label: 'New Name'},
  ];

  devices: any = [];
  hasSelectedHome = false;
  homeId: string = '';

  messageHome: string = '';
  messageDevice: string = '';
  isHomeSuccess: boolean = false;
  isDeviceSuccess: boolean = false;

  constructor(private homeDeviceService: HomeDeviceService, private homeOwnerService: HomeOwnerService) {
  }

  onHomeFormSubmit(homeFormData: any) {
    this.homeId = homeFormData.homeId;
    this.loadDevices(this.homeId);
  }

  onDeviceFormSubmit(deviceFormData: any) {
    const model: ModifyHomeDeviceNameModel = {
      Name: deviceFormData.name
    }
    let hardwareId = deviceFormData.hardwareId;
    this.homeDeviceService.modifyHomeDeviceName(model, hardwareId).subscribe(() => {
        this.loadDevices(this.homeId);
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageDevice = error.details;
        this.isDeviceSuccess = false;
      });
  }

  loadDevices(homeId: string) {
    this.homeOwnerService.getHomeDevices(homeId).subscribe((response: any) => {
        this.deviceFormElements[0].values = response.map((device: any) => device.homeDeviceId);
        this.deviceFormElements[0].options = response.map((device: any) => device.name == null ? '-' : device.name);
        this.hasSelectedHome = true;
        this.messageHome = '';
        this.isHomeSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageHome = error.details;
        this.isHomeSuccess = false;
      });
  }
}
