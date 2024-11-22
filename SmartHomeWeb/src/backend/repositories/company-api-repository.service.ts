import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from './api-repository.service';
import environments from '../../environments/environments.local';
import CreateCompanyModel from '../services/company-owner/models/CreateCompanyModel';
import CreateCameraModel from '../services/company-owner/models/CreateCameraModel';
import CreateDeviceModel from '../services/company-owner/models/CreateDeviceModel';
import ImportDevicesModel from '../services/company-owner/models/ImportDevicesModel';

@Injectable({
  providedIn: 'root',
})
export class CompanyRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  createCompany(model: CreateCompanyModel) {
    return this.post(model, 'companies');
  }

  createCamera(model: CreateCameraModel) {
    return this.post(model, 'cameras');
  }

  createMotionSensor(model: CreateDeviceModel) {
    return this.post(model, 'motionSensors');
  }

  createSmartLamp(model: CreateDeviceModel) {
    return this.post(model, 'smartLamps');
  }

  createWindowSensor(model: CreateDeviceModel) {
    return this.post(model, 'windowSensors');
  }

  changeRole() {
    return this.patch('changeCompanyOwnerRoles');
  }

  getModelValidators() {
    return this.get('modelValidators');
  }

  getDeviceImporters() {
    return this.get('deviceImporters');
  }

  importDevices(request: ImportDevicesModel, dllId: string) {
    return this.post(request, `${dllId}/importDevices`);
  }

  getDeviceImporterParameters(dllId: string) {
    return this.get(`deviceImporters/${dllId}/parameters`);
  }
}
