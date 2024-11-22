import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateWindowSensorComponent} from './create-window-sensor-page/create-window-sensor.component';
import {CreateWindowSensorRoutingModule} from './create-window-sensor-routing.module';
import {CreateDeviceComponent} from '../../../business-components/create-device/create-device.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateWindowSensorComponent],
  imports: [
    CommonModule,
    CreateWindowSensorRoutingModule,
    CreateDeviceComponent,
    MainMenuComponent,
    ServerResponseVisualization
  ],
})
export class CreateWindowSensorModule {
}
