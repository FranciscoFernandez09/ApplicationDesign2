import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {GetCompaniesComponent} from './get-companies-page/get-companies.component';


const routes: Routes = [
  {
    path: '',
    component: GetCompaniesComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GetCompaniesRoutingModule {
}
