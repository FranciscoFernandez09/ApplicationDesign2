import {NgModule} from '@angular/core';
import {CommonModule, NgOptimizedImage} from '@angular/common';
import {GetDevicesComponent} from './get-devices-page/get-devices.component';
import {GetDevicesRoutingModule} from './get-devices-routing.module';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {TableNavigatorComponent} from "../../../components/table-navigator/table-navigator.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [GetDevicesComponent],
  imports: [CommonModule, GetDevicesRoutingModule, MainMenuComponent, TableNavigatorComponent, ServerResponseVisualization, NgOptimizedImage],
})
export class GetDevicesModule {
}
