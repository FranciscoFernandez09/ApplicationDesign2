import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {DeleteAdminComponent} from './delete-admin-page/delete-admin.component';


const routes: Routes = [
  {
    path: '',
    component: DeleteAdminComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DeleteAdminRoutingModule {
}
