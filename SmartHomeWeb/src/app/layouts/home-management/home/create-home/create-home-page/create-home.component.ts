import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import CreateHomeModel from '../../../../../../backend/services/home-management/home-owner/models/CreateHomeModel';

@Component({
  selector: 'app-create-home',
  templateUrl: './create-home.component.html',
  styleUrl: './create-home.component.css',
  providers: [HomeOwnerService],
})
export class CreateHomeComponent {
  formElements = [
    {
      name: 'name', type: 'text', label: 'Name',
    },
    {name: 'addressStreet', type: 'text', label: 'Address Street', required: true},
    {name: 'addressNumber', type: 'number', label: 'Address Number', required: true},
    {name: 'latitude', type: 'number', label: 'Latitude', required: true},
    {name: 'longitude', type: 'number', label: 'Longitude', required: true},
    {name: 'maxMembers', type: 'number', label: 'Max Members', required: true},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService) {
  }

  onFormSubmit(formData: any) {
    const createHomeModel: CreateHomeModel = {
      name: formData.name,
      addressStreet: formData.addressStreet,
      addressNumber: formData.addressNumber,
      latitude: formData.latitude,
      longitude: formData.longitude,
      maxMembers: formData.maxMembers,
    }
    this.homeOwnerService.createHome(createHomeModel).subscribe(
      (response: any) => {
        console.log('Home created:', response);
        this.message = response;
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}
