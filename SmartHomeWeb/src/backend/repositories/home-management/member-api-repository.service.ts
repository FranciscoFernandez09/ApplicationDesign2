import {Injectable} from '@angular/core';
import environments from '../../../environments/environments.local';
import {HttpClient} from '@angular/common/http';
import ApiRepositoryService from '../api-repository.service';
import FilterNotificationRequest from '../../services/home-management/member/models/FilterNotificationRequest';

@Injectable({providedIn: 'root',})

export class MemberApiRepositoryService extends ApiRepositoryService {
  constructor(http: HttpClient) {
    super(environments.smartHomeApi, '', http);
  }

  activateMemberNotification(memberId: string) {
    return this.patch(`members/${memberId}/activateNotification`);
  }

  deactivateMemberNotification(memberId: string) {
    return this.patch(`members/${memberId}/deactivateNotification`);
  }

  getNotifications(request: FilterNotificationRequest) {
    const query = `deviceType=${request.DeviceType}&date=${request.Date}&isRead=${request.IsRead}`;
    return this.get('notifications', query);
  }
}
