import {Injectable} from '@angular/core';
import {UnknownUserApiRepositoryService} from '../../repositories/unknown-user-api-repository.service';
import CreateUserModel from './models/CreateUserModel';

@Injectable({
  providedIn: 'root'
})
export class UnknownUserService {
  constructor(private readonly _repository: UnknownUserApiRepositoryService) {
  }

  public createHomeOwner(model: CreateUserModel) {
    return this._repository.createHomeOwner(model);
  }
}
