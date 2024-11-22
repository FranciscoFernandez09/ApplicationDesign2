import {Component} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';

@Component({
  selector: 'app-create-motion-sensor',
  templateUrl: './create-motion-sensor.component.html',
  styleUrl: './create-motion-sensor.component.css',
  providers: [CompanyOwnerService],
})
export class CreateMotionSensorComponent {
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _companyOwnerService: CompanyOwnerService) {
  }

  onDeviceSubmit(newDevice: any) {
    console.log('Device:', newDevice);
    this._companyOwnerService
      .createMotionSensor(newDevice)
      .subscribe((response: any) => {
          console.log('Motion sensor created:', response);
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
