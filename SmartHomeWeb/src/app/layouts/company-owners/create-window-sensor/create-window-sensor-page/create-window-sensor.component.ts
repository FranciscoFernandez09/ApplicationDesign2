import {Component} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';

@Component({
  selector: 'app-create-smart-lamp',
  templateUrl: './create-window-sensor.component.html',
  styleUrl: './create-window-sensor.component.css',
  providers: [CompanyOwnerService],
})
export class CreateWindowSensorComponent {
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _companyOwnerService: CompanyOwnerService) {
  }

  onDeviceSubmit(newDevice: any) {
    console.log('Device:', newDevice);
    this._companyOwnerService
      .createWindowSensor(newDevice)
      .subscribe((response: any) => {
          console.log('Window sensor created:', response);
          this.message = response;
          this.isSuccess = true;
        },
        (error) => {
          console.error('Error:', error);
          this.message = error.details;
          this.isSuccess = false;
        });
  }
}
