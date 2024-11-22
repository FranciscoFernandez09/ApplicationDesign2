import {Component, OnInit} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import AddHomePermissionModel
  from '../../../../../../backend/services/home-management/home-owner/models/AddHomePermissionModel';

@Component({
  selector: 'app-add-home-permission',
  templateUrl: './add-home-permission.component.html',
  styleUrl: './add-home-permission.component.css',
  providers: [HomeOwnerService],
})
export class AddHomePermissionComponent implements OnInit {
  permissions: any = [];
  selectedMember: string = '';
  hasSelectHome: boolean = false;

  formElements = [
    {name: 'memberId', type: 'select-with-values', label: 'Member', values: [], options: []},
    {name: 'permissionId', type: 'select-with-values', label: 'Permission', values: [], options: []},
  ];
  messageHome: string = '';
  message: string = '';
  isHomeSuccess: boolean = false;
  isSuccess: boolean = false;


  constructor(private homeOwnerService: HomeOwnerService) {
  }

  ngOnInit() {
    this.loadPermissions();
  }

  onHomeSubmit(formData: any) {
    this.homeOwnerService.getHomeMembers(formData.homeId).subscribe((response: any) => {
        this.formElements[0].values = response.map((member: any) => member.homeMemberId);
        this.formElements[0].options = response.map((member: any) => member.fullName);
        this.hasSelectHome = true;
        this.messageHome = '';
        this.isHomeSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageHome = error.details;
        this.isHomeSuccess = false;
      }
    );
  }

  onFormSubmit(formData: any) {
    const addHomePermission: AddHomePermissionModel = {
      PermissionId: formData.permissionId,
    }
    console.log('Add home permission:', addHomePermission);
    this.selectedMember = formData.memberId;
    this.homeOwnerService.addHomePermission(addHomePermission, this.selectedMember).subscribe(
      (response: any) => {
        console.log('Home permission added:', response);
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

  loadPermissions() {
    this.homeOwnerService.getHomePermissions().subscribe(
      (response: any) => {
        this.permissions = response;
        this.formElements[1].options = this.permissions.map((permission: any) => permission.name);
        this.formElements[1].values = this.permissions.map((permission: any) => permission.id);
      },
      (error: any) => {
        console.error('Error loading permissions:', error);
      });
  }

}
