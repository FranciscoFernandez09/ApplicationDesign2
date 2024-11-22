import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ManageMemberNotificationRoutingModule} from './manage-member-notification-routing.module';
import {
  ManageMemberNotificationComponent
} from './manage-member-notification-page/manage-member-notification.component';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from "../../../../business-components/load-homes/load-homes.component";

@NgModule({
  declarations: [ManageMemberNotificationComponent],
  imports: [CommonModule, MainMenuComponent, ManageMemberNotificationRoutingModule, FormComponent, ServerResponseVisualization, LoadHomesComponent],
})
export class ManageMemberNotificationModule {
}
