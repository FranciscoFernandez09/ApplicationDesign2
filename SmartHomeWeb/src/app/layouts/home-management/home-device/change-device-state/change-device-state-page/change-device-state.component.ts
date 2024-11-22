import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import {HomeDeviceService} from '../../../../../../backend/services/home-management/home-device/home-device.service';

@Component({
  selector: 'change-device-state',
  templateUrl: './change-device-state.component.html',
  styleUrls: ['./change-device-state.component.css'],
})
export class ChangeDeviceStateComponent {

  deviceFormElements = [
    {name: 'hardwareId', type: 'select-with-values', label: 'Device', values: [], options: []},
  ];

  devices: any = [];
  hasSelectedHome = false;
  hasSelectedDevice = false;

  homeId: string = '';
  hardwareId: string = '';

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
    this.hardwareId = deviceFormData.hardwareId;
    this.hasSelectedDevice = true;
  }

  onActionFormSubmit(action: string) {
    const actionObservable = action === 'TurnOn'
      ? this.homeDeviceService.connectDevice(this.hardwareId)
      : this.homeDeviceService.disconnectDevice(this.hardwareId);

    actionObservable.subscribe(
      (response: any) => {
        this.messageDevice = response;
        this.isDeviceSuccess = true;
        this.loadDevices(this.homeId);
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageDevice = error.details;
        this.isDeviceSuccess = false;
      }
    );
  }

  loadDevices(homeId: string) {
    this.homeOwnerService.getHomeDevices(homeId).subscribe((response: any) => {
        this.deviceFormElements[0].values = response.map((device: any) => device.homeDeviceId);
        this.deviceFormElements[0].options = response.map((device: any) => `${device.name} (${device.isConnected ? 'Connected' : 'Disconnected'})`);
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
