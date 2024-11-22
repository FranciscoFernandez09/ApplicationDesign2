import {Injectable} from '@angular/core';
import ApiRepositoryService from './api-repository.service';
import {HttpClient} from '@angular/common/http';
import environments from '../../environments/environments.local';
import FilterDeviceModel from '../services/user/models/FilterDeviceModel';
import ModifyUserProfileImageModel from '../services/user/models/ModifyUserProfileImage';

@Injectable({
  providedIn: 'root',
})
export class UserApiService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  public getDevices(model: FilterDeviceModel) {
    let query = `name=${model.name}&model=${model.model}&companyName=${model.companyName}&type=${model.type}&offset=${model.offset}&limit=${model.limit}`;
    return this.get('devices', query);
  }

  public getDeviceTypes() {
    return this.get('deviceTypes');
  }

  public modifyUserProfileImage(model: ModifyUserProfileImageModel) {
    return this.patch('users/profileImage', model);
  }
}
