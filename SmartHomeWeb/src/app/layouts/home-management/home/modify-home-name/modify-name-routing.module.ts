import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ModifyNameComponent} from './modify-name-page/modify-name.component';


const routes: Routes = [
  {
    path: '',
    component: ModifyNameComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ModifyNameRoutingModule {
}
