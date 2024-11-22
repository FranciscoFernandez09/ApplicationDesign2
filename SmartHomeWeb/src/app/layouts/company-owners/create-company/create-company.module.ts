import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateCompanyComponent} from './create-company-page/create-company.component';
import {FormComponent} from '../../../components/form/form.component';
import {CreateCompanyRoutingModule} from './create-company-routing.module';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateCompanyComponent],
  imports: [CommonModule, CreateCompanyRoutingModule, FormComponent, MainMenuComponent, ServerResponseVisualization],
})
export class CreateCompanyModule {
}
