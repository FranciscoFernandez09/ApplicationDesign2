import {Component, OnInit} from '@angular/core';
import {UserService} from '../../../../../backend/services/user/user.service';
import FilterDeviceModel from '../../../../../backend/services/user/models/FilterDeviceModel';

@Component({
  selector: 'app-get-devices',
  templateUrl: './get-devices.component.html',
  styleUrl: './get-devices.component.css',
  providers: [UserService],
})
export class GetDevicesComponent implements OnInit {
  headers = ["Name", "Model", "Main Image", "Company Owner"];
  devices: any = [];
  types: any = [];

  formElements = [
    {name: 'name', type: 'text', label: 'Name'},
    {name: 'model', type: 'text', label: 'Model'},
    {name: 'companyName', type: 'text', label: 'Company Name'},
    {name: 'type', type: 'select-with-values', label: 'Type', options: []},
  ];

  message: string = '';
  isSuccess: boolean = false;

  constructor(private _userService: UserService) {
  }

  ngOnInit(): void {
    this.loadDeviceTypes();
    const filter: FilterDeviceModel = {
      name: '',
      model: '',
      companyName: '',
      type: '',
      offset: 0,
      limit: 10,
    };
    this._userService.getDevices(filter).subscribe(
      (response) => {
        this.devices = response;
        this.message = '';
        this.isSuccess = true;
      }, (error) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }

  onFormSubmit(data: any) {
    console.log('Form Data:', data);
    let formData = data.formData;
    const filter: FilterDeviceModel = {
      name: formData.name ?? '',
      model: formData.model ?? '',
      companyName: formData.companyName ?? '',
      type: formData.type ?? '',
      offset: data.offset ?? 0,
      limit: data.limit ?? 10,
    }
    this._userService.getDevices(filter).subscribe(
      (response) => {
        console.log('Devices:', response);
        this.devices = response;
        this.message = '';
        this.isSuccess = true;
      }, (error) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false
      });
  }

  loadDeviceTypes() {
    this._userService.getDeviceTypes().subscribe(
      (response) => {
        this.types = response;
        this.formElements[3].options = this.types;
        this.isSuccess = true;
        this.message = '';
      },
      (error) => {
        console.error('Error:', error);
        this.message = error.details;
        this.isSuccess = false;
      });
  }
}

