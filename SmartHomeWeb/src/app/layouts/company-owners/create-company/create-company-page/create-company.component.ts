import {Component, OnInit} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';
import CreateCompanyModel from '../../../../../backend/services/company-owner/models/CreateCompanyModel';

@Component({
  selector: 'app-create-company',
  templateUrl: './create-company.component.html',
  styleUrl: './create-company.component.css',
  providers: [CompanyOwnerService],
})
export class CreateCompanyComponent implements OnInit {
  validators: any = [];
  formElements = [
    {name: 'name', type: 'text', label: 'Name'},
    {name: 'rut', type: 'text', label: 'Rut'},
    {name: 'logo', type: 'text', label: 'Logo'},
    {
      name: 'validators',
      type: 'select-with-values',
      label: 'Select Device Model Validator',
      options: [],
      values: [],
    },
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private _companyOwnerService: CompanyOwnerService) {
  }

  ngOnInit(): void {
    this.loadValidators();
  }

  onFormSubmit(formData: any) {
    console.log('Form Data:', formData);
    const newCompany: CreateCompanyModel = {
      name: formData.name,
      rut: formData.rut,
      logo: formData.logo,
      validatorId: formData.validators,
    };
    this._companyOwnerService
      .createCompany(newCompany)
      .subscribe((response: any) => {
          console.log('Company created:', response);
          this.message = response;
          this.isSuccess = true;
        },
        (error) => {
          console.error('Error:', error);
          this.message = error.details;
          this.isSuccess = false;
        }
      );
  }

  loadValidators() {
    this._companyOwnerService.getModelValidators().subscribe(
      (response) => {
        this.validators = response;
        this.formElements[3].options = this.validators.map((validator: any) => `${validator.name}`);
        this.formElements[3].values = this.validators.map((validator: any) => validator.id);
      },
      (error) => {
        console.error('Error loading validators:', error);
      }
    );
  }
}
