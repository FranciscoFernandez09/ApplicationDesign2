import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ChangeDeviceStateComponent} from './change-device-state-page/change-device-state.component';
import {ChangeDeviceStateRoutingModule} from './change-device-state-routing.module';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from "../../../../business-components/load-homes/load-homes.component";

@NgModule({
  declarations: [ChangeDeviceStateComponent],
  imports: [CommonModule, ChangeDeviceStateRoutingModule, MainMenuComponent, FormComponent, ServerResponseVisualization, LoadHomesComponent],
})
export class ChangeDeviceStateModule {
}
