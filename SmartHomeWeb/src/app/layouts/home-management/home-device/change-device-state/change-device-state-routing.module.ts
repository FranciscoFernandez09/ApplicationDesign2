import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ChangeDeviceStateComponent} from './change-device-state-page/change-device-state.component';


const routes: Routes = [
  {
    path: '',
    component: ChangeDeviceStateComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ChangeDeviceStateRoutingModule {
}
