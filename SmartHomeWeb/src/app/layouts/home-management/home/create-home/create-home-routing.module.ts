import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateHomeComponent} from './create-home-page/create-home.component';


const routes: Routes = [
  {
    path: '',
    component: CreateHomeComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateHomeRoutingModule {
}
