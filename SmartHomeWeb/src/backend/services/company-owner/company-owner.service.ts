import {Injectable} from '@angular/core';
import {CompanyRepositoryService} from '../../repositories/company-api-repository.service';
import CreateCompanyModel from './models/CreateCompanyModel';
import CreateCameraModel from './models/CreateCameraModel';
import CreateDeviceModel from './models/CreateDeviceModel';
import ImportDevicesModel from './models/ImportDevicesModel';

@Injectable({
  providedIn: 'root',
})
export class CompanyOwnerService {
  constructor(private readonly _repository: CompanyRepositoryService) {
  }

  public createCompany(request: CreateCompanyModel) {
    return this._repository.createCompany(request);
  }

  public createCamera(request: CreateCameraModel) {
    return this._repository.createCamera(request);
  }

  public createMotionSensor(request: CreateDeviceModel) {
    return this._repository.createMotionSensor(request);
  }

  public createSmartLamp(request: CreateDeviceModel) {
    return this._repository.createSmartLamp(request);
  }

  public createWindowSensor(request: CreateDeviceModel) {
    return this._repository.createWindowSensor(request);
  }

  public changeRole() {
    return this._repository.changeRole();
  }

  public getModelValidators() {
    return this._repository.getModelValidators();
  }

  public getDeviceImporters() {
    return this._repository.getDeviceImporters();
  }

  public importDevices(request: ImportDevicesModel, dllId: string) {
    return this._repository.importDevices(request, dllId);
  }

  public getDeviceImporterParameters(dllId: string): any {
    return this._repository.getDeviceImporterParameters(dllId);
  }
}
