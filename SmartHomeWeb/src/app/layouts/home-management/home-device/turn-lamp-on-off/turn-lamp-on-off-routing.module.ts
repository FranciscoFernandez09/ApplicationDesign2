import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TurnLampOnOffComponent} from './turn-lamp-on-off-page/turn-lamp-on-off.component';

const routes: Routes = [
  {
    path: '',
    component: TurnLampOnOffComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TurnLampOnOffRoutingModule {
}
