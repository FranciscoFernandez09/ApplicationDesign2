import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LandingComponent} from './layouts/landing/landing-page/landing.component';
import {authGuard} from '../guards/auth.guard';

export const routes: Routes = [
  {
    path: 'sign-in',
    loadChildren: () =>
      import('./layouts/authentication/authentication.module').then(
        (m) => m.AuthenticationModule
      ),
  },
  {
    path: 'register',
    loadChildren: () =>
      import('./layouts/register/register.module').then(
        (m) => m.RegisterModule
      ),
  },
  {
    path: 'welcome',
    loadChildren: () =>
      import('./layouts/welcome/welcome.module').then((m) => m.WelcomeModule),
  },
  {
    path: 'landing',
    loadChildren: () =>
      import('./layouts/landing/landing.module').then((m) => m.LandingModule),
  },
  {
    path: 'companies',
    loadChildren: () =>
      import(
        './layouts/company-owners/create-company/create-company.module'
        ).then((m) => m.CreateCompanyModule),
  },
  {
    path: 'notifications',
    loadChildren: () =>
      import('./layouts/home-management/member/get-notifications/get-notifications.module').then(
        (m) => m.GetNotificationsModule
      ),
  },

  /*------------------------------ ADMIN SERVICE -----------------------------*/
  {
    path: 'admin/create',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/create-admin/create-admin.module').then(
        (m) => m.CreateAdminModule
      ),
  },
  {
    path: 'admin/delete',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/delete-admin/delete-admin.module').then(
        (m) => m.DeleteAdminModule
      ),
  },
  {
    path: 'admin/company-owner',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/admins/create-company-owner/create-company-owner.module'
        ).then((m) => m.CreateCompanyOwnerModule),
  },
  {
    path: 'admin/users',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/get-users/get-users.module').then(
        (m) => m.GetUsersModule
      ),
  },
  {
    path: 'admin/companies',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/get-companies/get-companies.module').then(
        (m) => m.GetCompaniesModule
      ),
  },
  {
    path: 'admin/change-role',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/change-role/change-role.module').then(
        (m) => m.ChangeRoleModule
      ),
  },

  /*------------------------------ COMPANY OWNER SERVICE -----------------------------*/
  {
    path: 'company-owner/company',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/company-owners/create-company/create-company.module'
        ).then((m) => m.CreateCompanyModule),
  },
  {
    path: 'company-owner/camera',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/company-owners/create-camera/create-camera.module'
        ).then((m) => m.CreateCameraModule),
  },
  {
    path: 'company-owner/motion-sensor',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/company-owners/create-motion-sensor/create-motion-sensor.module'
        ).then((m) => m.CreateMotionSensorModule),
  },
  {
    path: 'company-owner/smart-lamp',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/company-owners/create-smart-lamp/create-smart-lamp.module'
        ).then((m) => m.CreateSmartLampModule),
  },
  {
    path: 'company-owner/window-sensor',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/company-owners/create-window-sensor/create-window-sensor.module'
        ).then((m) => m.CreateWindowSensorModule),
  },
  {
    path: 'company-owner/change-role',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/company-owners/change-role/change-role.module').then(
        (m) => m.ChangeRoleModule
      ),
  },
  {
    path: 'admin/company-owner',
    canActivate: [authGuard],
    loadChildren: () =>
      import(
        './layouts/admins/create-company-owner/create-company-owner.module'
        ).then((m) => m.CreateCompanyOwnerModule),
  },
  {
    path: 'admin/users',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/admins/get-users/get-users.module').then(
        (m) => m.GetUsersModule
      ),
  },
  {
    path: 'company-owner/import-devices',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/company-owners/import-devices/import-devices.module').then(
        (m) => m.ImportDevicesModule
      ),
  },

  /*------------------------------ HOME OWNER SERVICE -----------------------------*/
  {
    path: 'home-owner/add-device',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/add-device/add-device.module').then(m => m.AddDeviceModule),
  },
  {
    path: 'home-owner/add-home-permission',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/add-home-permission/add-home-permission.module').then(m => m.AddHomePermissionModule),
  },
  {
    path: 'home-owner/add-member',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/add-member/add-member.module').then(m => m.AddMemberModule),
  },
  {
    path: 'home-owner/get-home-devices',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/get-home-devices/get-home-devices.module').then(m => m.GetHomeDevicesModule),
  },
  {
    path: 'home-owner/get-home-member',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/get-home-members/get-home-members.module').then(m => m.GetHomeMembersModule),
  },
  {
    path: 'home-owner/modify-home-name',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/modify-home-name/modify-name.module').then(m => m.ModifyNameModule),
  },
  {
    path: 'home-owner/create-home',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home/create-home/create-home.module').then(m => m.CreateHomeModule),
  },
  {
    path: 'home-owner/add-room',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/room/add-room/add-room.module').then(m => m.AddRoomModule),
  },
  {
    path: 'home-owner/add-device-to-room',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/room/add-device-to-room/add-device-to-room.module').then(m => m.AddDeviceToRoomModule),
  },
  {
    path: 'home-owner/modify-home-device-name',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home-device/modify-home-device-name/modify-home-device-name.module').then(m => m.ModifyHomeDeviceNameModule),
  },
  {
    path: 'home-owner/change-device-state',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home-device/change-device-state/change-device-state.module').then(m => m.ChangeDeviceStateModule),
  },
  {
    path: 'home-owner/manage-member-notifications',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/member/manage-member-notification/manage-member-notification.module').then(m => m.ManageMemberNotificationModule),
  },
  {
    path: 'home-owner/manage-lamp',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/home-management/home-device/turn-lamp-on-off/turn-lamp-on-off.module').then(m => m.TurnLampOnOffModule),
  },
  /*------------------------------ USER SERVICE -----------------------------*/
  {
    path: 'users/get-devices',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/users/get-devices/get-devices.module').then(m => m.GetDevicesModule),
  },
  {
    path: 'users/get-device-types',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/users/get-device-types/get-device-types.module').then(m => m.GetDeviceTypesModule),
  },
  {
    path: 'users/modify-profileImage',
    canActivate: [authGuard],
    loadChildren: () =>
      import('./layouts/users/modify-profile-image/modify-profile-image.module').then(m => m.ModifyProfileImageModule),
  },
  {
    path: '**',
    component: LandingComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
