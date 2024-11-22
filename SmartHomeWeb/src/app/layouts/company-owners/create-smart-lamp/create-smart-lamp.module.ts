import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateSmartLampComponent} from './create-smart-lamp-page/create-smart-lamp.component';
import {CreateSmartLampRoutingModule} from './create-smart-lamp-routing.module';
import {CreateDeviceComponent} from '../../../business-components/create-device/create-device.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateSmartLampComponent],
  imports: [CommonModule, CreateSmartLampRoutingModule, CreateDeviceComponent, MainMenuComponent, ServerResponseVisualization],
})
export class CreateSmartLampModule {
}
