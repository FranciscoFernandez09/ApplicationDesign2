import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateCompanyOwnerComponent} from './create-company-owner-page/create-company-owner.component';
import {FormComponent} from '../../../components/form/form.component';
import {CreateCompanyOwnerRoutingModule} from './create-company-owner-routing.module';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateCompanyOwnerComponent],
  imports: [
    CommonModule,
    CreateCompanyOwnerRoutingModule,
    FormComponent,
    MainMenuComponent,
    ServerResponseVisualization,
  ],
})
export class CreateCompanyOwnerModule {
}
