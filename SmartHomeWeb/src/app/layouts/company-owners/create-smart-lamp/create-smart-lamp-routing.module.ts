import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateSmartLampComponent} from './create-smart-lamp-page/create-smart-lamp.component';

const routes: Routes = [
  {
    path: '',
    component: CreateSmartLampComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateSmartLampRoutingModule {
}
