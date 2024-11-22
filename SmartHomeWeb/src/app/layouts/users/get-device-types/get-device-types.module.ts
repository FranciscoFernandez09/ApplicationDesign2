import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {GetDeviceTypesComponent} from './get-device-types-page/get-device-types.component';
import {GetDeviceTypesRoutingModule} from './get-device-types-routing.module';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {TableNavigatorComponent} from "../../../components/table-navigator/table-navigator.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [GetDeviceTypesComponent],
  imports: [CommonModule, GetDeviceTypesRoutingModule, MainMenuComponent, TableNavigatorComponent, ServerResponseVisualization],
})
export class GetDeviceTypesModule {
}
