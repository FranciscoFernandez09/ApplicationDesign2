import {Component} from '@angular/core';
import {UnknownUserService} from '../../../../backend/services/unknown-user/unknown-user.service';
import CreateUserModel from '../../../../backend/services/unknown-user/models/CreateUserModel';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  providers: [],
})
export class RegisterComponent {
  formElements = [
    {name: 'name', type: 'text', label: 'Name', required: true},
    {name: 'lastName', type: 'text', label: 'Last Name', required: true},
    {name: 'email', type: 'text', label: 'Email', required: true},
    {name: 'password', type: 'password', label: 'Password', required: true},
    {name: 'profileImage', type: 'text', label: 'Profile Image', required: true},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private unknownUserService: UnknownUserService, private router: Router) {
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    const newHomeOwner: CreateUserModel = {
      name: formData.name,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      profileImage: formData.profileImage,
    };
    this.unknownUserService.createHomeOwner(newHomeOwner).subscribe(
      (response: any) => {
        console.log('Response:', response);
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

  navigateToLogin() {
    this.router.navigate(['/login']);
  }
}
