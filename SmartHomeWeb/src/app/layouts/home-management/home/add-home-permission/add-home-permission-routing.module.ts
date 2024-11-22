import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AddHomePermissionComponent} from './add-home-permission-page/add-home-permission.component';


const routes: Routes = [
  {
    path: '',
    component: AddHomePermissionComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AddHomePermissionRoutingModule {
}
