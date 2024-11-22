import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {TurnLampOnOffRoutingModule} from './turn-lamp-on-off-routing.module';
import {MainMenuComponent} from '../../../../business-components/navmenus/main-menu/main-menu.component';
import {FormComponent} from '../../../../components/form/form.component';
import {
  ServerResponseVisualization
} from "../../../../business-components/server-response-visualization/server-response-visualization";
import {LoadHomesComponent} from '../../../../business-components/load-homes/load-homes.component';
import {TurnLampOnOffComponent} from './turn-lamp-on-off-page/turn-lamp-on-off.component';

@NgModule({
  declarations: [TurnLampOnOffComponent],
  imports: [CommonModule, TurnLampOnOffRoutingModule, MainMenuComponent, FormComponent, ServerResponseVisualization, LoadHomesComponent],
})
export class TurnLampOnOffModule {
}
