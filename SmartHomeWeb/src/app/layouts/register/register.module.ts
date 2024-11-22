import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RegisterComponent} from './register-page/register.component';
import {FormComponent} from '../../components/form/form.component';
import {RegisterRoutingModule} from './register-routing.module';
import {
  ServerResponseVisualization
} from "../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [RegisterComponent],
  imports: [CommonModule, RegisterRoutingModule, FormComponent, ServerResponseVisualization],
})
export class RegisterModule {
}
