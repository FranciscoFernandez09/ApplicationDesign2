import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import GetUsersModel from '../../../../../backend/services/admin/models/GetUsersModel';

@Component({
  selector: 'app-get-users',
  templateUrl: './get-users.component.html',
  styleUrl: './get-users.component.css',
  providers: [AdminService],
})
export class GetUsersComponent implements OnInit {
  headers = ["CreatedAt", "Name", "Last Name", "Role"];
  users: any = [];
  formElements = [
    {name: 'name', type: 'text', label: 'Name'},
    {name: 'lastName', type: 'text', label: 'Last Name'},
    {name: '', type: 'note', label: 'Filter is by full-name, fill both fields'},
    {name: 'role', type: 'text', label: 'Role'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _adminService: AdminService) {
  }

  ngOnInit(): void {
    const filter: GetUsersModel = {
      name: '',
      lastName: '',
      role: '',
      offset: 0,
      limit: 10,
    };
    this._adminService.getUsers(filter).subscribe((response) => {
      console.log('Users:', response);
      this.users = response;
    });
  }

  onFormSubmit(data: any) {
    console.log('Form Data:', data);
    let formData = data.formData;
    const filter: GetUsersModel = {
      name: formData.name?? '',
      lastName: formData.lastName?? '',
      role: formData.role?? '',
      offset: data.offset?? 0,
      limit: data.limit?? 10,
    };
    this._adminService
      .getUsers(filter)
      .subscribe((response) => {
          console.log('Users:', response);
          this.users = response;
        },
        (error) => {
          console.error('Error:', error);
          this.message = error.details;
          this.isSuccess = false
        });
  }
}
