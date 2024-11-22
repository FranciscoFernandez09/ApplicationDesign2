import {Component} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import CreateCompanyOwnerModel from '../../../../../backend/services/admin/models/CreateCompanyOwnerModel';

@Component({
  selector: 'app-create-company-owner',
  templateUrl: './create-company-owner.component.html',
  styleUrl: './create-company-owner.component.css',
  providers: [AdminService],
})
export class CreateCompanyOwnerComponent {
  formElements = [
    {name: 'name', type: 'text', label: 'Name', required: true},
    {name: 'lastName', type: 'text', label: 'Last Name', required: true},
    {name: 'email', type: 'text', label: 'Email', required: true},
    {name: 'password', type: 'password', label: 'Password', required: true},
    {name: 'profileImage', type: 'text', label: 'Profile Image'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _adminService: AdminService) {
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    const newCompanyOwner: CreateCompanyOwnerModel = {
      name: formData.name,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      profileImage: formData.profileImage,
    };
    this._adminService
      .createCompanyOwner(newCompanyOwner)
      .subscribe((response: any) => {
        console.log('CompanyOwner created:', response);
        this.message = response;
        this.isSuccess = true;
      }, (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}
