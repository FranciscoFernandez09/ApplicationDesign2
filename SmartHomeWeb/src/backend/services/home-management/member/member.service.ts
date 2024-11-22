import {Injectable} from '@angular/core';
import {MemberApiRepositoryService} from '../../../repositories/home-management/member-api-repository.service';
import FilterNotificationModel from './models/FilterNotificationModel';
import {DatePipe} from '@angular/common';
import FilterNotificationRequest from './models/FilterNotificationRequest';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  constructor(private readonly _repository: MemberApiRepositoryService) {
  }

  activateMemberNotification(memberId: string,) {
    return this._repository.activateMemberNotification(memberId);
  }

  deactivateMemberNotification(memberId: string) {
    return this._repository.deactivateMemberNotification(memberId);
  }

  getNotifications(model: FilterNotificationModel) {
    console.log(model);
    let request: FilterNotificationRequest = {
      DeviceType: model.DeviceType,
      Date: this.validateDate(model.Date),
      IsRead: model.IsRead
    }
    return this._repository.getNotifications(request);
  }

  private validateDate(date: Date | null): string {
    if (!date || isNaN(date.getTime())) {
      return '';
    }
    const datePipe = new DatePipe('en-US');
    let parsedDate: Date | null = date ? new Date(date) : null;
    if (parsedDate) {
      parsedDate.setDate(parsedDate.getDate() + 1);
    }
    let result = parsedDate ? datePipe.transform(parsedDate, 'yyyy-MM-dd') : '';
    return result ? result : '';
  }
}
