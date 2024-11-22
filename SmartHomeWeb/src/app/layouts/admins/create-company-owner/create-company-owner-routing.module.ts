import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateCompanyOwnerComponent} from './create-company-owner-page/create-company-owner.component';


const routes: Routes = [
  {
    path: '',
    component: CreateCompanyOwnerComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateCompanyOwnerRoutingModule {
}
