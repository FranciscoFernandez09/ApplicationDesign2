import {Component, OnInit} from '@angular/core';
import {HomeOwnerService} from '../../../../../../backend/services/home-management/home-owner/home-owner.service';
import ModifyHomeNameModel
  from '../../../../../../backend/services/home-management/home-owner/models/ModifyHomeNameModel';

@Component({
  selector: 'app-modify-name',
  templateUrl: './modify-name.component.html',
  styleUrl: './modify-name.component.css',
  providers: [HomeOwnerService],
})
export class ModifyNameComponent implements OnInit {
  formElements = [
    {name: 'homeId', type: 'select-with-values', label: 'Home', values: [], options: []},
    {name: 'name', type: 'text', label: 'New Name'},
  ];
  message: string = '';
  isSuccess: boolean = false;

  constructor(private homeOwnerService: HomeOwnerService) {
  }

  ngOnInit() {
    this.loadHomes();
  }

  onFormSubmit(formData: any) {
    const modifyHomeName: ModifyHomeNameModel = {
      name: formData.name
    }
    this.homeOwnerService.modifyHomeName(modifyHomeName, formData.homeId).subscribe(
      (response: any) => {
        console.log('Home name modified:', response);
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
      this.formElements[0].values = response.map((home: any) => home.homeId);
      this.formElements[0].options = response.map((home: any) => home.name);
    });
  }

}
