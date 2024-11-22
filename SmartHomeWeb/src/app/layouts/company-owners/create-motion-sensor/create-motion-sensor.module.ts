import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateMotionSensorComponent} from './create-motion-sensor-page/create-motion-sensor.component';
import {CreateMotionSensorRoutingModule} from './create-motion-sensor-routing.module';
import {
  ExceptionVisualizationComponent
} from '../../../components/exception-visualization/exception-visualization.component';
import {CreateDeviceComponent} from "../../../business-components/create-device/create-device.component";
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateMotionSensorComponent],
  imports: [CommonModule, CreateMotionSensorRoutingModule, ExceptionVisualizationComponent, CreateDeviceComponent, MainMenuComponent, ServerResponseVisualization],
})
export class CreateMotionSensorModule {
}
