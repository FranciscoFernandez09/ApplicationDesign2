import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import environments from '../../environments/environments.local';
import ApiRepositoryService from './api-repository.service';


@Injectable({
  providedIn: 'root',
})
export class DeviceActionApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  public turnLampOn(hardwareId: string) {
    return this.patch(`smartLamps/${hardwareId}/turnOn`);
  }

  public turnLampOff(hardwareId: string) {
    return this.patch(`smartLamps/${hardwareId}/turnOff`);
  }


}
