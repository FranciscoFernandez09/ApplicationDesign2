import {Component, OnInit} from '@angular/core';
import {MemberService} from '../../../../../../backend/services/home-management/member/member.service';
import {UserService} from '../../../../../../backend/services/user/user.service';
import FilterNotificationModel
  from '../../../../../../backend/services/home-management/member/models/FilterNotificationModel';

@Component({
  selector: 'app-notification-popover',
  templateUrl: './get-notifications.component.html',
  styleUrls: ['./get-notifications.component.css'],
})

export class GetNotificationsComponent implements OnInit {
  notifications: any = [];
  headers: any = ['Event', 'Hardware Id', 'Date', 'Is Read'];
  devices: any = [];
  messageNotification: string = '';
  isNotificationSuccess: boolean = false;
  messageDevice: string = '';
  isDeviceSuccess: boolean = false;


  formElements: any = [
    {name: 'deviceType', type: 'select-with-values', label: 'Device', values: [], options: []},
    {name: 'date', type: 'date', label: 'Date'},
    {name: 'isRead', type: 'radio', options: ['Both', 'Yes', 'No'], label: 'Filter by read status'},
  ];

  constructor(private memberService: MemberService, private userService: UserService) {
  }


  ngOnInit(): void {
    this.loadDevicesTypes();
  }

  showNotifications(deviceType: string, date: string, isRead: string = '') {
    let parsedDate: Date = new Date(date);
    let isReadFilter: string = (isRead === 'Yes') ? 'true' : (isRead === 'No') ? 'false' : '';
    let model: FilterNotificationModel = {
      DeviceType: deviceType === 'All' ? '' : deviceType,
      Date: parsedDate,
      IsRead: isReadFilter
    }
    this.memberService.getNotifications(model).subscribe(
      (response: any) => {
        this.notifications = [];
        this.notifications = response;
        this.isNotificationSuccess = true;
        this.messageNotification = '';
      },
      (error: any) => {
        console.error('Error loading notifications:', error);
        this.messageNotification = error.details;
      });
  }

  onFormSubmit(formData: any) {
    console.log(formData);
    this.showNotifications(formData.deviceType, formData.date, formData.isRead);
  }

  loadDevicesTypes() {
    this.userService.getDeviceTypes().subscribe(
      (response: any) => {
        console.log(response);
        this.formElements[0].values = response;
        this.formElements[0].options = response;
        this.formElements[0].options.push('All');
        this.isDeviceSuccess = true;
        this.messageDevice = '';
      },
      (error: any) => {
        console.error('Error loading device types:', error);
        this.messageDevice = error.details;
        this.isDeviceSuccess = false;
      });
  }
}
