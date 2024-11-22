import {Component} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import CreateAdminModel from '../../../../../backend/services/admin/models/CreateAdminModel';

@Component({
  selector: 'app-create-admin',
  templateUrl: './create-admin.component.html',
  styleUrl: './create-admin.component.css',
  providers: [AdminService],
})
export class CreateAdminComponent {
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
    const newAdmin: CreateAdminModel = {
      name: formData.name,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      profileImage: formData.profileImage,
    };
    this._adminService
      .createAdmin(newAdmin)
      .subscribe((response: any) => {
          console.log('Admin created:', response);
          this.message = response;
          this.isSuccess = true;
        },
        (error: any) => {
          console.error('Error creating admin:', error);
          this.message = error.details;
          this.isSuccess = false;
        });
  }
}
