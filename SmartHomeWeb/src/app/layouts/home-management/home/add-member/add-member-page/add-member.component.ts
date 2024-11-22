import {Component, OnInit} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import AddMemberModel from '../../../../../../backend/services/home-management/home-owner/models/AddMemberModel';

@Component({
  selector: 'app-add-member',
  templateUrl: './add-member.component.html',
  styleUrl: './add-member.component.css',
  providers: [HomeOwnerService],
})
export class AddMemberComponent implements OnInit {
  formElements = [
    {name: 'memberEmail', type: 'text', label: 'User email'},
    {name: 'homeId', type: 'select-with-values', label: 'Home', values: [], options: []},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService) {
  }

  ngOnInit() {
    this.loadHomes();
  }

  onFormSubmit(formData: any) {
    const addMember: AddMemberModel = {
      MemberEmail: formData.memberEmail,
    }
    this.homeOwnerService.addMember(addMember, formData.homeId).subscribe(
      (response: any) => {
        console.log('Member added:', response);
        this.message = response;
        this.isSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }

  loadHomes() {
    this.homeOwnerService.getMineHomes().subscribe((response: any) => {
        this.formElements[1].values = response.map((home: any) => home.homeId);
        this.formElements[1].options = response.map((home: any) => home.name);
      },
      (error: any) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }


}
