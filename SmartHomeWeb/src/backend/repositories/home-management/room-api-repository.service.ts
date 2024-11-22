import {Injectable} from '@angular/core';
import environments from '../../../environments/environments.local';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from '../api-repository.service';
import AddRoomModel from '../../services/home-management/room/models/AddRoomModel';
import AddDeviceToRoomModel from '../../services/home-management/room/models/AddDeviceToRoomModel';

@Injectable({providedIn: 'root',})

export class RoomApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  addRoom(homeId: string, model: AddRoomModel) {
    return this.post(model, `homes/${homeId}/rooms`);
  }

  addDeviceToRoom(roomId: string, model: AddDeviceToRoomModel) {
    return this.post(model, `rooms/${roomId}/devices`);
  }

  getRooms(homeId: string) {
    return this.get(`${homeId}/rooms`);
  }
}
