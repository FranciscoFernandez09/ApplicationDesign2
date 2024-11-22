import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {AddDeviceComponent} from './add-device-page/add-device.component';
import {AddDeviceRoutingModule} from './add-device-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from "../../../../business-components/load-homes/load-homes.component";
import {NgForOf, NgOptimizedImage} from '@angular/common';
import {TableNavigatorComponent} from '../../../../components/table-navigator/table-navigator.component';

@NgModule({
  declarations: [AddDeviceComponent],
  imports: [
    FormComponent,
    AddDeviceRoutingModule,
    MainMenuComponent,
    ServerResponseVisualization,
    LoadHomesComponent,
    NgOptimizedImage,
    NgForOf,
    TableNavigatorComponent,
  ],
})
export class AddDeviceModule {
}
