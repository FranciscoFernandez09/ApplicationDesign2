import {NgModule} from '@angular/core';
import {CreateHomeComponent} from './create-home-page/create-home.component';
import {FormComponent} from '../../../../components/form/form.component';
import {CreateHomeRoutingModule} from './create-home-routing.module';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateHomeComponent],
  imports: [
    FormComponent,
    FormComponent,
    CreateHomeRoutingModule,
    MainMenuComponent,
    ServerResponseVisualization,
  ],
})
export class CreateHomeModule {
}
