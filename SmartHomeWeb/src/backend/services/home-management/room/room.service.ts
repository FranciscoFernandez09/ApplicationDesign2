import {Injectable} from '@angular/core';
import {RoomApiRepositoryService} from '../../../repositories/home-management/room-api-repository.service';
import AddRoomModel from './models/AddRoomModel';
import AddDeviceToRoomModel from './models/AddDeviceToRoomModel';


@Injectable({
  providedIn: 'root'
})
export class RoomService {
  constructor(private readonly _repository: RoomApiRepositoryService) {
  }

  public addRoom(homeId: string, model: AddRoomModel) {
    return this._repository.addRoom(homeId, model);
  }

  public addDeviceToRoom(roomId: string, model: AddDeviceToRoomModel) {
    return this._repository.addDeviceToRoom(roomId, model);
  }

  public getRooms(homeId: string) {
    return this._repository.getRooms(homeId);
  }
}
