import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetHomeMembersComponent} from './get-home-members-page/get-home-members.component';


const routes: Routes = [
  {
    path: '',
    component: GetHomeMembersComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetHomeMembersRoutingModule {
}
