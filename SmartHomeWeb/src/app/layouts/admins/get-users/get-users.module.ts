import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {GetUsersComponent} from './get-users-page/get-users.component';
import {FormComponent} from '../../../components/form/form.component';
import {GetUsersRoutingModule} from './get-users-routing.module';
import {TableNavigatorComponent} from '../../../components/table-navigator/table-navigator.component';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [GetUsersComponent],
  imports: [CommonModule, GetUsersRoutingModule, FormComponent, TableNavigatorComponent, MainMenuComponent, ServerResponseVisualization],
})
export class GetUsersModule {
}
