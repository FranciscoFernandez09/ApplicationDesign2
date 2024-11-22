import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import DeleteAdminModel from '../../../../../backend/services/admin/models/DeleteAdminModel';
import GetUsersModel from '../../../../../backend/services/admin/models/GetUsersModel';

@Component({
  selector: 'app-delete-admin',
  templateUrl: './delete-admin.component.html',
  styleUrls: ['./delete-admin.component.css'],
  providers: [AdminService],
})
export class DeleteAdminComponent implements OnInit {
  users: any = [];
  userElements: { name: string; type: string; label: string; options?: string[]; }[] = [
    {name: 'name', type: 'text', label: 'Name',},
    {name: 'lastName', type: 'text', label: 'Last Name',},];

  message: string = '';
  isSuccess: boolean = false;
  offset: number = 0;
  limit: number = 10;
  name: string = '';
  lastName: string = '';

  constructor(private _adminService: AdminService) {
  }

  ngOnInit() {
    this.loadUsers()
  }

  onPageChange(formData: any) {
    this.limit = formData.limit ?? formData.limit;
    this.offset = formData.offset ?? formData.offset;
    this.name = formData.formData.name ?? formData.name;
    this.lastName = formData.formData.lastName ?? formData.lastName;

    this.loadUsers();
  }

  loadUsers() {
    //check for undefined and return it to 0,10 or ''
    this.offset = this.offset ?? 0;
    this.limit = this.limit ?? 10;
    this.name = this.name ?? '';
    this.lastName = this.lastName ?? '';

    let model: GetUsersModel = {
      name: this.name,
      lastName: this.lastName,
      role: 'Admin',
      offset: this.offset,
      limit: this.limit,
    };
    this._adminService.getUsers(model).subscribe(
      (response) => {
        this.users = response;
      },
      (error) => {
        console.error('Error fetching users:', error);
      }
    );
  }

  deleteAdmin(adminsId: string) {
    const selectedAdmin: DeleteAdminModel = {
      adminId: adminsId,
    };
    this._adminService.deleteAdmin(selectedAdmin).subscribe(
      (response: any) => {
        console.log('Admin deleted:', response);
        this.message = response;
        this.isSuccess = true;
        this.loadUsers()
      },
      (error) => {
        console.error('Error deleting admin:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }
}
