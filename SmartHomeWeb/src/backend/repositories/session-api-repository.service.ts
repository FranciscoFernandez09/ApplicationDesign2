import {Injectable} from '@angular/core';
import ApiRepositoryService from './api-repository.service';
import {HttpClient} from '@angular/common/http';
import environments from '../../environments/environments.local';
import UserCredentialsModel from '../services/session/models/UserCredentialsModel';
import SessionCreatedModel from '../services/session/models/SessionCreatedModel';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SessionApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, 'sessions', http);
  }

  public login(credentials: UserCredentialsModel): Observable<SessionCreatedModel> {
    return this.post(credentials);
  }

  public logout(): void {
    this.delete();
  }
}
