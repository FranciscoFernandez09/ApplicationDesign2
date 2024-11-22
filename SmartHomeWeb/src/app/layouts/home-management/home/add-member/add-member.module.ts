import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {AddMemberComponent} from './add-member-page/add-member.component';
import {AddMemberRoutingModule} from './add-member-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [AddMemberComponent],
  imports: [
    FormComponent,
    AddMemberRoutingModule,
    MainMenuComponent,
    ServerResponseVisualization,
  ],
})
export class AddMemberModule {
}
