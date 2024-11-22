import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {GetHomeMembersComponent} from './get-home-members-page/get-home-members.component';
import {GetHomeMembersRoutingModule} from './get-home-members-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {TableNavigatorComponent} from '../../../../components/table-navigator/table-navigator.component';
import {NgForOf, NgIf} from '@angular/common';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from "../../../../business-components/load-homes/load-homes.component";

@NgModule({
  declarations: [GetHomeMembersComponent],
  imports: [
    FormComponent,
    GetHomeMembersRoutingModule,
    MainMenuComponent,
    TableNavigatorComponent,
    NgIf,
    NgForOf,
    ServerResponseVisualization,
    LoadHomesComponent,
  ],
})
export class GetHomeMembersModule {
}
