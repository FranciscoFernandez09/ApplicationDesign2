import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import {HomeDeviceService} from '../../../../../../backend/services/home-management/home-device/home-device.service';
import {DeviceActionService} from '../../../../../../backend/services/device-action/device-action.service';

@Component({
  selector: 'change-device-state',
  templateUrl: './turn-lamp-on-off.component.html',
  styleUrls: ['./turn-lamp-on-off.component.css'],
})
export class TurnLampOnOffComponent {

  deviceFormElements = [
    {name: 'hardwareId', type: 'select-with-values', label: 'Smart lamps', values: [], options: []},
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

  constructor(private deviceActionService: DeviceActionService, private homeOwnerService: HomeOwnerService) {
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
      ? this.deviceActionService.turnLampOn(this.hardwareId)
      : this.deviceActionService.turnLampOff(this.hardwareId);

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
        let smartLamps = response.filter((device: any) => device.type === 'SmartLamp');
        console.log('Smart Lamps:', smartLamps);
        this.deviceFormElements[0].values = smartLamps.map((device: any) => device.homeDeviceId);
        this.deviceFormElements[0].options = smartLamps.map((device: any) => device.name?? '-' + ' ' + (device.isActive ? 'On' : 'Off'));
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
