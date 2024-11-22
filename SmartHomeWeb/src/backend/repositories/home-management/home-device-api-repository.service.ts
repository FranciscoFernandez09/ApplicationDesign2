import {Injectable} from '@angular/core';
import environments from '../../../environments/environments.local';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from '../api-repository.service';
import ModifyHomeDeviceNameModel from '../../services/home-management/home-device/models/ModifyHomeDeviceNameModel';

@Injectable({providedIn: 'root',})

export class HomeDeviceApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, 'hardwares', http);
  }

  modifyHomeDeviceName(model: ModifyHomeDeviceNameModel, hardwareId: string) {
    return this.patch(`${hardwareId}/name`, {Name: model.Name});
  }

  disconnectDevice(hardwareId: string) {
    return this.patch(`${hardwareId}/disconnection`);
  }

  connectDevice(hardwareId: string) {
    return this.patch(`${hardwareId}/connection`);
  }

}
