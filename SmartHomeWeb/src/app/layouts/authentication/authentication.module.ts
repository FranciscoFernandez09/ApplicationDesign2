import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AuthenticationRoutingModule} from './authentication-routing.module';
import {AuthenticationPageComponent} from './authentication-page/authentication-page.component';
import {SignInComponent} from './sign-in/sign-in.component';
import {FormComponent} from '../../components/form/form.component';
import {RouterModule} from '@angular/router';
import {
  ExceptionVisualizationComponent
} from "../../components/exception-visualization/exception-visualization.component";
import {SuccessVisualizationComponent} from '../../components/success-visualization/success-visualization.component';
import {
  ServerResponseVisualization
} from '../../business-components/server-response-visualization/server-response-visualization';

@NgModule({
  declarations: [AuthenticationPageComponent, SignInComponent],
  imports: [
    CommonModule,
    AuthenticationRoutingModule,
    FormComponent,
    RouterModule,
    ExceptionVisualizationComponent,
    SuccessVisualizationComponent,
    ServerResponseVisualization,
  ],
})
export class AuthenticationModule {
}
