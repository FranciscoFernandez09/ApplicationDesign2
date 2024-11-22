import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from '../api-repository.service';
import environments from '../../../environments/environments.local';
import CreateHomeModel from '../../services/home-management/home-owner/models/CreateHomeModel';
import AddMemberModel from '../../services/home-management/home-owner/models/AddMemberModel';
import AddHomePermissionModel from '../../services/home-management/home-owner/models/AddHomePermissionModel';
import AddDeviceModel from '../../services/home-management/home-owner/models/AddDeviceModel';
import ModifyHomeNameModel from '../../services/home-management/home-owner/models/ModifyHomeNameModel';

@Injectable({
  providedIn: 'root',
})
export class HomeApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, 'homes', http);
  }

  public createHome(model: CreateHomeModel) {
    return this.post(model);
  }

  public addMember(model: AddMemberModel, homeId: string) {
    return this.post(model, `${homeId}/members`);
  }

  public addHomePermission(model: AddHomePermissionModel, memberId: string) {
    return this.post(model, `${memberId}/permissions`);
  }

  public addDevice(model: AddDeviceModel, homeId: string) {
    return this.post(model, `${homeId}/devices`);
  }

  public getHomeMembers(homeId: string) {
    return this.get(`${homeId}/members`);
  }

  public getHomeDevices(homeId: string, roomId = '') {
    let query = roomId ? `room=${roomId}` : '';
    return this.get(`${homeId}/devices`, query);
  }

  public modifyHomeName(model: ModifyHomeNameModel, homeId: string) {
    return this.patch(`${homeId}/name`, model);
  }

  public getMineHomes() {
    return this.get(`mine`);
  }

  public getHomesWhereIMember() {
    return this.get(`member`);
  }

  public getHomePermissions() {
    return this.get('permissions');
  }
}
