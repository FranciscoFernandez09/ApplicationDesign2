import {Component} from '@angular/core';
import {UserService} from '../../../../../backend/services/user/user.service';
import ModifyUserProfileImageModel from '../../../../../backend/services/user/models/ModifyUserProfileImage';

@Component({
  selector: 'app-modify-profile-image',
  templateUrl: './modify-profile-image.component.html',
  styleUrl: './modify-profile-image.component.css',
  providers: [UserService],
})
export class ModifyProfileImageComponent {
  formElements = [
    {name: 'profileImage', type: 'text', label: 'New Profile Image'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private service: UserService) {
  }

  onFormSubmit(formData: any) {
    const model: ModifyUserProfileImageModel = {
      profileImage: formData.profileImage
    }
    this.service.modifyUserProfileImage(model).subscribe(
      (response: any) => {
        this.message = response;
        this.isSuccess = true;
      }, (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }

}
