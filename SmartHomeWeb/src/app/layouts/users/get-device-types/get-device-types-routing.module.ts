import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetDeviceTypesComponent} from './get-device-types-page/get-device-types.component';

const routes: Routes = [
  {
    path: '',
    component: GetDeviceTypesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetDeviceTypesRoutingModule {
}
