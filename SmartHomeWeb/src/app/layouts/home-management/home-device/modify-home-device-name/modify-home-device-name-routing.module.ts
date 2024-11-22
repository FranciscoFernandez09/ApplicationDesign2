import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ModifyHomeDeviceNameComponent} from './modify-home-device-name-page/modify-home-device-name.component';

const routes: Routes = [
  {
    path: '',
    component: ModifyHomeDeviceNameComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ModifyHomeDeviceNameRoutingModule {
}
