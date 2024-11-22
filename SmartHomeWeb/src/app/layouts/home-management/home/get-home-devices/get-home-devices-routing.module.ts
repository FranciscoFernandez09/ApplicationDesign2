import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetHomeDevicesComponent} from './get-home-devices-page/get-home-devices.component';


const routes: Routes = [
  {
    path: '',
    component: GetHomeDevicesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetHomeDevicesRoutingModule {
}
