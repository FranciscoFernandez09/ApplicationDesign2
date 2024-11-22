import {Injectable} from '@angular/core';
import {HomeApiRepositoryService} from '../../../repositories/home-management/home-api-repository.service';
import CreateHomeModel from './models/CreateHomeModel'
import AddMemberModel from './models/AddMemberModel';
import AddHomePermissionModel from './models/AddHomePermissionModel';
import AddDeviceModel from './models/AddDeviceModel';
import ModifyHomeNameModel from './models/ModifyHomeNameModel';


@Injectable({
  providedIn: 'root'
})
export class HomeOwnerService {
  constructor(private readonly _repository: HomeApiRepositoryService) {
  }

  public createHome(model: CreateHomeModel) {
    return this._repository.createHome(model);
  }

  public addMember(model: AddMemberModel, homeId: string) {
    return this._repository.addMember(model, homeId);
  }

  public addHomePermission(model: AddHomePermissionModel, memberId: string) {
    return this._repository.addHomePermission(model, memberId);
  }

  public addDevice(model: AddDeviceModel, homeId: string) {
    return this._repository.addDevice(model, homeId);
  }

  public getHomeMembers(homeId: string) {
    return this._repository.getHomeMembers(homeId);
  }

  public getHomeDevices(homeId: string, roomId = '') {
    return this._repository.getHomeDevices(homeId, roomId);
  }

  public modifyHomeName(model: ModifyHomeNameModel, homeId: string) {
    return this._repository.modifyHomeName(model, homeId);
  }

  public getMineHomes() {
    return this._repository.getMineHomes();
  }

  public getHomesWhereIMember() {
    return this._repository.getHomesWhereIMember();
  }

  public getHomePermissions() {
    return this._repository.getHomePermissions();
  }
}
