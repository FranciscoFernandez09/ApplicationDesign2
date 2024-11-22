import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LandingComponent} from './landing-page/landing.component';
import {FormComponent} from '../../components/form/form.component';
import {LandingRoutingModule} from './landing-routing.module';
import {RouterButtonsComponent} from '../../components/router-buttons/router-buttons.component';

@NgModule({
  declarations: [LandingComponent],
  imports: [
    CommonModule,
    LandingRoutingModule,
    FormComponent,
    RouterButtonsComponent,
  ],
})
export class LandingModule {
}
