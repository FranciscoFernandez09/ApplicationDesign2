import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateAdminComponent} from './create-admin-page/create-admin.component';
import {FormComponent} from '../../../components/form/form.component';
import {CreateAdminRoutingModule} from './create-admin-routing.module';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from '../../../business-components/server-response-visualization/server-response-visualization';

@NgModule({
  declarations: [CreateAdminComponent],
  imports: [CommonModule, CreateAdminRoutingModule, FormComponent, MainMenuComponent, ServerResponseVisualization],
})
export class CreateAdminModule {
}
