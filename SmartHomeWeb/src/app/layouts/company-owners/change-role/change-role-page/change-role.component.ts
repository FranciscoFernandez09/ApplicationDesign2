import {Component} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-create-camera',
  templateUrl: './change-role.component.html',
  styleUrl: './change-role.component.css',
  providers: [CompanyOwnerService],
})
export class ChangeRoleComponent {
  message: string = '';
  isSuccess: boolean = false;

  constructor(private companyOwnerService: CompanyOwnerService, private router: Router) {
  }

  changeRole() {
    this.companyOwnerService.changeRole().subscribe(
      (response: any) => {
        console.log('Change Role:', response);
        this.message = response;
        this.isSuccess = true;
        // delay 3seg
        this.router.navigate(['/landing']);
      },
      (error) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}
