import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {ModifyNameComponent} from './modify-name-page/modify-name.component';
import {ModifyNameRoutingModule} from './modify-name-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [ModifyNameComponent],
  imports: [
    FormComponent,
    ModifyNameRoutingModule,
    MainMenuComponent,
    ServerResponseVisualization,
  ],
})
export class ModifyNameModule {
}
