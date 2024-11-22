import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateWindowSensorComponent} from './create-window-sensor-page/create-window-sensor.component';

const routes: Routes = [
  {
    path: '',
    component: CreateWindowSensorComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateWindowSensorRoutingModule {
}
