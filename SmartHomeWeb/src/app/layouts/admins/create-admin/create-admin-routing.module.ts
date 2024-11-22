import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateAdminComponent} from './create-admin-page/create-admin.component';


const routes: Routes = [
  {
    path: '',
    component: CreateAdminComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateAdminRoutingModule {
}
