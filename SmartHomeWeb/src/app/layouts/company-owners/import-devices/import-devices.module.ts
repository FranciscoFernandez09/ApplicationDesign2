import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ImportDevicesComponent} from './import-devices-page/import-devices.component';
import {ImportDevicesRoutingModule} from './import-devices.routing.module';
import {CreateDeviceComponent} from '../../../business-components/create-device/create-device.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {FormComponent} from '../../../components/form/form.component';
import {
  ServerResponseVisualization
} from '../../../business-components/server-response-visualization/server-response-visualization';

@NgModule({
  declarations: [ImportDevicesComponent],
  imports: [
    CommonModule,
    ImportDevicesRoutingModule,
    CreateDeviceComponent,
    MainMenuComponent,
    FormComponent,
    ServerResponseVisualization
  ],
})
export class ImportDevicesModule {
}
