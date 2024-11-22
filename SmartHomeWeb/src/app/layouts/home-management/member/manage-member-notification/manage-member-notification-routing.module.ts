import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {
  ManageMemberNotificationComponent
} from './manage-member-notification-page/manage-member-notification.component';

const routes: Routes = [
  {
    path: '',
    component: ManageMemberNotificationComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ManageMemberNotificationRoutingModule {
}
