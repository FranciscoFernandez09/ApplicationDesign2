import {Component} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';

@Component({
  selector: 'app-get-home-member',
  templateUrl: './get-home-members.component.html',
  styleUrl: './get-home-members.component.css',
  providers: [HomeOwnerService],
})
export class GetHomeMembersComponent {

  members: any = [];
  hasSelectedHome = false;
  message: string = '';
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService) {
  }

  onFormSubmit(formData: any) {
    this.hasSelectedHome = false;
    this.homeOwnerService.getHomeMembers(formData.homeId).subscribe((members: any) => {
        this.hasSelectedHome = true;
        this.members = members;
        this.message = '';
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}
