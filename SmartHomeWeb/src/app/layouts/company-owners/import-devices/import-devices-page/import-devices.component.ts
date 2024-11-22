import {Component, OnInit} from '@angular/core';
import {CompanyOwnerService} from '../../../../../backend/services/company-owner/company-owner.service';
import ImportDevicesModel from '../../../../../backend/services/company-owner/models/ImportDevicesModel';

interface ShowParamDto {
  key: string;
  value: string;
}

@Component({
  selector: 'import-devices',
  templateUrl: './import-devices.component.html',
  styleUrls: ['./import-devices.component.css'],
})
export class ImportDevicesComponent implements OnInit {
  reflectionFormElements = [
    {name: 'reflectionId', type: 'select-with-values', label: 'Importer', values: [], options: []},
  ];
  parametersFormElements: { name: string; type: string; label: string; required: boolean }[] = [];

  devices: any = [];

  refId: string = '';
  hardwareId: string = '';
  hasSelectedRef: boolean = false;

  messageRef: string = '';
  message: string = '';
  IsRefSuccess: boolean = false;
  isSuccess: boolean = false;

  constructor(private companyOwnerService: CompanyOwnerService) {
  }

  ngOnInit() {
    this.loadReflections();
  }

  onRefFormSubmit(homeFormData: any) {
    this.refId = homeFormData.reflectionId;
    this.showImporterParamsForm(this.refId);
  }

  onParamsFormSubmit(deviceFormData: any) {
    const params: ImportDevicesModel = {parameters: {}};
    this.parametersFormElements.forEach((element: any) => {
      params.parameters[element.name] = deviceFormData[element.name];
    });

    this.message = 'Importing devices...';
    this.isSuccess = true;
    this.companyOwnerService.importDevices(params, this.refId).subscribe(
      (response: any) => {
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

  loadReflections() {
    this.companyOwnerService.getDeviceImporters().subscribe(
      (response: any) => {
        this.reflectionFormElements[0].values = response.map((ref: any) => ref.dllId);
        this.reflectionFormElements[0].options = response.map((ref: any) => ref.dllName);
        this.messageRef = '';
        this.IsRefSuccess = true;
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageRef = error.details;
        this.IsRefSuccess = false;
      }
    );
  }

  showImporterParamsForm(refId: string) {
    this.parametersFormElements = [];
    this.companyOwnerService.getDeviceImporterParameters(refId).subscribe(
      (response: ShowParamDto[]) => {
        this.hasSelectedRef = true;
        this.parametersFormElements = response.map((param: ShowParamDto) => ({
          name: param.key,
          type: "text",
          label: param.key.charAt(0).toUpperCase() + param.key.slice(1),
          required: true,
        }));
      },
      (error: any) => {
        console.error('Error:', error);
        this.messageRef = error.details;
        this.IsRefSuccess = false;
      }
    );
  }
}
