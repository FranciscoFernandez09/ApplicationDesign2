import {Injectable} from '@angular/core';
import {UserApiService} from '../../repositories/user-api-repository.service';
import FilterDeviceModel from './models/FilterDeviceModel';
import ModifyUserProfileImageModel from './models/ModifyUserProfileImage';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private readonly _repository: UserApiService) {
  }

  public getDevices(model: FilterDeviceModel) {
    return this._repository.getDevices(model);
  }

  public getDeviceTypes() {
    return this._repository.getDeviceTypes();
  }

  public modifyUserProfileImage(model: ModifyUserProfileImageModel) {
    return this._repository.modifyUserProfileImage(model);
  }
}
