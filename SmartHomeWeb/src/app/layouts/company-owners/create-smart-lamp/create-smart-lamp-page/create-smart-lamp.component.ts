import {Component} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';

@Component({
  selector: 'app-create-smart-lamp',
  templateUrl: './create-smart-lamp.component.html',
  styleUrl: './create-smart-lamp.component.css',
  providers: [CompanyOwnerService],
})
export class CreateSmartLampComponent {
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _companyOwnerService: CompanyOwnerService) {
  }

  onDeviceSubmit(newDevice: any) {
    console.log('Device:', newDevice);
    this._companyOwnerService
      .createSmartLamp(newDevice)
      .subscribe((response: any) => {
          console.log('Smart lamp created:', response);
          this.message = response;
          this.isSuccess = true;
        },
        (error) => {
          console.error('Error:', error);
          this.message = error.details;
          this.isSuccess = false;
        }
      );
  }
}
