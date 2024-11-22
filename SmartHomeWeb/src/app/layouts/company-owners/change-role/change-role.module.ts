import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ChangeRoleRoutingModule} from './change-role-routing.module';
import {ChangeRoleComponent} from './change-role-page/change-role.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [ChangeRoleComponent],
  imports: [CommonModule, ChangeRoleRoutingModule, MainMenuComponent, ServerResponseVisualization],
})
export class ChangeRoleModule {
}
