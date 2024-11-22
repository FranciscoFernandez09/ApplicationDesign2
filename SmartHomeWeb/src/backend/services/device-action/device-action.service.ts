import {Injectable} from '@angular/core';
import {DeviceActionApiRepositoryService} from '../../repositories/device-action-api-repository.service';

@Injectable({
  providedIn: 'root'
})
export class DeviceActionService {
  constructor(private readonly _repository: DeviceActionApiRepositoryService) {
  }

  public turnLampOn(hardwareId: string) {
    return this._repository.turnLampOn(hardwareId);
  }

  public turnLampOff(hardwareId: string) {
    return this._repository.turnLampOff(hardwareId);
  }
}
