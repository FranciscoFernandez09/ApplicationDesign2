import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {AddDeviceToRoomRoutingModule} from './add-device-to-room-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {AddDeviceToRoomComponent} from './add-device-to-room-page/add-device-to-room.component';
import {NgIf} from '@angular/common';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from '../../../../business-components/load-homes/load-homes.component';

@NgModule({
  declarations: [AddDeviceToRoomComponent],
  imports: [FormComponent, AddDeviceToRoomRoutingModule, MainMenuComponent, NgIf, ServerResponseVisualization, LoadHomesComponent],
})
export class AddDeviceToRoomModule {
}
