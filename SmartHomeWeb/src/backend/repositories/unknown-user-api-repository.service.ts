import {Injectable} from '@angular/core';
import ApiRepositoryService from './api-repository.service';
import {HttpClient} from '@angular/common/http';
import environments from '../../environments/environments.local';
import CreateUserModel from '../services/unknown-user/models/CreateUserModel';

@Injectable({
  providedIn: 'root',
})
export class UnknownUserApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  public createHomeOwner(model: CreateUserModel) {
    return this.post(model, 'homeOwners');
  }

}
