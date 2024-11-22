import {Injectable} from '@angular/core';
import {HomeDeviceApiRepositoryService} from '../../../repositories/home-management/home-device-api-repository.service';
import ModifyHomeDeviceNameModel from './models/ModifyHomeDeviceNameModel';


@Injectable({
  providedIn: 'root'
})
export class HomeDeviceService {
  constructor(private readonly _repository: HomeDeviceApiRepositoryService) {
  }

  public modifyHomeDeviceName(model: ModifyHomeDeviceNameModel, hardwareId: string) {
    return this._repository.modifyHomeDeviceName(model, hardwareId);
  }

  public disconnectDevice(hardwareId: string) {
    return this._repository.disconnectDevice(hardwareId);
  }

  public connectDevice(hardwareId: string) {
    return this._repository.connectDevice(hardwareId);
  }
}
