import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetNotificationsComponent} from './get-notification-page/get-notifications.component';

const routes: Routes = [
  {
    path: '',
    component: GetNotificationsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetNotificationsRoutingModule {
}
