import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {AddHomePermissionComponent} from './add-home-permission-page/add-home-permission.component';
import {AddHomePermissionRoutingModule} from './add-home-permission-routing.module';
import {NgIf} from "@angular/common";
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from '../../../../business-components/load-homes/load-homes.component';

@NgModule({
  declarations: [AddHomePermissionComponent],
  imports: [
    FormComponent,
    AddHomePermissionRoutingModule,
    NgIf,
    MainMenuComponent,
    ServerResponseVisualization,
    LoadHomesComponent,
  ],
})
export class AddHomePermissionModule {
}
