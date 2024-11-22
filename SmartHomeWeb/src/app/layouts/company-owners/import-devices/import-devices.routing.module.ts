import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ImportDevicesComponent} from './import-devices-page/import-devices.component';

const routes: Routes = [
  {
    path: '',
    component: ImportDevicesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ImportDevicesRoutingModule {
}
