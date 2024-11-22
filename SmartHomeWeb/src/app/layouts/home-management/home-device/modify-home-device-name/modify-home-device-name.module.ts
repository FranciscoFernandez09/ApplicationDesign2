import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ModifyHomeDeviceNameComponent} from './modify-home-device-name-page/modify-home-device-name.component';
import {ModifyHomeDeviceNameRoutingModule} from './modify-home-device-name-routing.module';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from '../../../../business-components/load-homes/load-homes.component';

@NgModule({
  declarations: [ModifyHomeDeviceNameComponent],
  imports: [CommonModule, ModifyHomeDeviceNameRoutingModule, MainMenuComponent, FormComponent, ServerResponseVisualization, LoadHomesComponent],
})
export class ModifyHomeDeviceNameModule {
}
