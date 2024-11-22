import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {DeleteAdminComponent} from './delete-admin-page/delete-admin.component';
import {FormComponent} from '../../../components/form/form.component';
import {DeleteAdminRoutingModule} from './delete-admin-routing.module';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";
import {TableNavigatorComponent} from "../../../components/table-navigator/table-navigator.component";

@NgModule({
  declarations: [DeleteAdminComponent],
  imports: [CommonModule, DeleteAdminRoutingModule, FormComponent, MainMenuComponent, ServerResponseVisualization, TableNavigatorComponent]
})
export class DeleteAdminModule {
}
