import {NgModule} from '@angular/core';
import {FormComponent} from '../../../../components/form/form.component';
import {GetHomeDevicesComponent} from './get-home-devices-page/get-home-devices.component';
import {GetHomeDevicesRoutingModule} from './get-home-devices-routing.module';
import {MainMenuComponent} from "../../../../business-components/navmenus/main-menu/main-menu.component";
import {NgForOf, NgIf, NgOptimizedImage} from "@angular/common";
import {
  ExceptionVisualizationComponent
} from "../../../../components/exception-visualization/exception-visualization.component";
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from "../../../../business-components/load-homes/load-homes.component";

@NgModule({
  declarations: [GetHomeDevicesComponent],
  imports: [
    FormComponent,
    GetHomeDevicesRoutingModule,
    MainMenuComponent,
    NgForOf,
    NgIf,
    ExceptionVisualizationComponent,
    ServerResponseVisualization,
    LoadHomesComponent,
    NgOptimizedImage,
  ],
})
export class GetHomeDevicesModule {
}
