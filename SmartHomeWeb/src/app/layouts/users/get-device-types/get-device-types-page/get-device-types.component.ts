import {Component, OnInit} from '@angular/core';
import {UserService} from '../../../../../backend/services/user/user.service';

@Component({
  selector: 'app-get-device-types',
  templateUrl: './get-device-types.component.html',
  styleUrl: './get-device-types.component.css',
  providers: [UserService],
})
export class GetDeviceTypesComponent implements OnInit {
  headers = ["Device Type"];
  types: any = [];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _userService: UserService) {
  }

  ngOnInit(): void {
    this.loadDeviceTypes();
  }

  loadDeviceTypes() {
    this._userService.getDeviceTypes().subscribe(
      (response) => {
        this.types = response;
        this.isSuccess = true;
      },
      (error) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      }
    );
  }
}

