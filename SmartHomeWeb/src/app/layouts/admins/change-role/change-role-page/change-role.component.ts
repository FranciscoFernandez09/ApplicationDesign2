import {Component} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-change-role',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.css',
  providers: [AdminService],
})
export class ChangeRoleComponent {
  message: string = '';
  isSuccess: boolean = false;

  constructor(private adminService: AdminService, private router: Router) {
  }

  changeRole() {
    this.adminService.changeRole().subscribe(
      (response: any) => {
        this.message = response;
        this.isSuccess = true;
        // Deley 3 seg
        this.router.navigate(['/landing']);
      },
      (error) => {
        console.log(error);
        this.isSuccess = false;
        this.message = error.details;
      }
    );
  }
}
