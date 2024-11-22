import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetUsersComponent} from './get-users-page/get-users.component';


const routes: Routes = [
  {
    path: '',
    component: GetUsersComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetUsersRoutingModule {
}
