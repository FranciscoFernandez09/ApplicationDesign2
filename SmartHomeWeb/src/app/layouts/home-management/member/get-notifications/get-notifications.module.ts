import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {GetNotificationsRoutingModule} from './get-notifications-routing.module';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {GetNotificationsComponent} from './get-notification-page/get-notifications.component';
import {TableNavigatorComponent} from '../../../../components/table-navigator/table-navigator.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [GetNotificationsComponent],
  imports: [CommonModule, MainMenuComponent, GetNotificationsRoutingModule, FormComponent, TableNavigatorComponent, ServerResponseVisualization],
})
export class GetNotificationsModule {
}
