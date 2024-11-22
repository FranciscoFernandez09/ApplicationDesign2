import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import {MemberService} from '../../../../../../backend/services/home-management/member/member.service';

@Component({
  selector: 'app-manage-member-notification',
  templateUrl: './manage-member-notification.component.html',
  styleUrls: ['./manage-member-notification.component.css'],
})
export class ManageMemberNotificationComponent {
  formElements: any = [
    {name: 'memberId', type: 'select-with-values', label: 'Members', options: [], values: []},
    {name: 'isRead', type: 'radio', label: 'Enable notifications', options: ["Yes", "No"], values: [true, false]},
  ];
  homeId: string = '';
  hasSelectedHome: boolean = false;

  messageHome: string = '';
  isSuccessHome: boolean = false;

  message: string = '';
  isSuccess: boolean = false;


  constructor(private homeOwnerService: HomeOwnerService, private memberService: MemberService) {
  }

  onHomeSubmit(formData: any) {
    this.homeId = formData.homeId;
    this.homeOwnerService.getHomeMembers(formData.homeId).subscribe(
      (response: any) => {
        console.log('Members:', response);
        this.formElements[0].options = response.map((member: any) => member.fullName);
        this.formElements[0].values = response.map((member: any) => member.homeMemberId);
        this.hasSelectedHome = true;
        this.isSuccessHome = true;
        this.messageHome = '';
      },
      (error: any) => {
        console.error('Error loading members:', error);
        this.messageHome = error.details;
        this.isSuccessHome = false;
      });
  }

  onFormSubmit(formData: any) {
    if (formData.isRead === 'Yes') {
      this.memberService.activateMemberNotification(formData.memberId).subscribe(
        (response: any) => {
          console.log('Notification activated:', response);
          this.isSuccess = true;
          this.message = response;
        },
        (error: any) => {
          console.error('Error activating notification:', error);
          this.message = error.details;
          this.isSuccess = false;
        });
    } else if (formData.isRead === 'No') {
      this.memberService.deactivateMemberNotification(formData.memberId).subscribe(
        (response: any) => {
          console.log('Notification deactivated:', response);
          this.isSuccess = true;
          this.message = response;
        },
        (error: any) => {
          console.error('Error deactivating notification:', error);
          this.message = error.details;
          this.isSuccess = false;
        });
    } else {
      this.message = 'Please fill out the form';
      this.isSuccess = false;
    }
  }
}
