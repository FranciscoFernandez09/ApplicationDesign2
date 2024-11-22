import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetDevicesComponent} from './get-devices-page/get-devices.component';

const routes: Routes = [
  {
    path: '',
    component: GetDevicesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetDevicesRoutingModule {
}
