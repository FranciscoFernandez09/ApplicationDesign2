import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AddRoomRoutingModule} from './add-room-routing.module';
import {AddRoomComponent} from './add-room-page/add-room.component';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [AddRoomComponent],
  imports: [CommonModule, MainMenuComponent, AddRoomRoutingModule, FormComponent, ServerResponseVisualization],
})
export class AddRoomModule {
}
