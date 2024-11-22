import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {GetCompaniesComponent} from './get-companies-page/get-companies.component';
import {FormComponent} from '../../../components/form/form.component';
import {GetCompaniesRoutingModule} from './get-companies-routing.module';
import {TableNavigatorComponent} from '../../../components/table-navigator/table-navigator.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [GetCompaniesComponent],
  imports: [CommonModule, GetCompaniesRoutingModule, FormComponent, TableNavigatorComponent, MainMenuComponent, ServerResponseVisualization],
})
export class GetCompaniesModule {
}
