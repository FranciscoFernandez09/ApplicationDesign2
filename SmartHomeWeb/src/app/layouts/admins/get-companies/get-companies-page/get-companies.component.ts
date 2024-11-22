import {Component, OnInit} from '@angular/core';
import {AdminService} from '../../../../../backend/services/admin/admin.service';
import GetCompaniesModel from '../../../../../backend/services/admin/models/GetCompanies';

@Component({
  selector: 'app-get-companies',
  templateUrl: './get-companies.component.html',
  styleUrl: './get-companies.component.css',
  providers: [AdminService],
})
export class GetCompaniesComponent implements OnInit {
  headers = ["Name", "Owner Full Name", "Owner Email", "RUT"];
  companies: any = [];
  formElements = [
    {name: 'companyName', type: 'text', label: 'Company name'},
    {name: 'ownerName', type: 'text', label: 'Owner name'},
    {name: 'ownerLastName', type: 'text', label: 'Owner lastname'},
    {name: '', type: 'note', label: 'Filter is by full-name, fill both fields'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _adminService: AdminService) {
  }

  ngOnInit(): void {
    const filter: GetCompaniesModel = {
      companyName: '',
      ownerName: '',
      ownerLastName: '',
      offset: 0,
      limit: 5,
    };
    this._adminService.getCompanies(filter).subscribe((response) => {
      console.log('Companies:', response);
      this.companies = response;
    });
  }

  onFormSubmit(data: any) {
    console.log('Form Data:', data);
    let formData = data.formData;
    const filter: GetCompaniesModel = {
      companyName: formData.companyName?? '',
      ownerName: formData.ownerName?? '',
      ownerLastName: formData.ownerLastName?? '',
      offset: data.offset?? 0,
      limit: data.limit?? 10,
    };
    this._adminService
      .getCompanies(filter)
      .subscribe((response) => {
          console.log('Companies:', response);
          this.companies = response;
        },
        (error) => {
          console.error('Error:', error);
          this.message = error.details;
          this.isSuccess = false
        });
  }
}
