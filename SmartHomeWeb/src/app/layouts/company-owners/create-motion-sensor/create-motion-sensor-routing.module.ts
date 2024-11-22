import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateMotionSensorComponent} from './create-motion-sensor-page/create-motion-sensor.component';

const routes: Routes = [
  {
    path: '',
    component: CreateMotionSensorComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateMotionSensorRoutingModule {
}
