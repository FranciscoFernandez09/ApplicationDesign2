import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ModifyProfileImageComponent} from './modify-profile-image-page/modify-profile-image.component';


const routes: Routes = [
  {
    path: '',
    component: ModifyProfileImageComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ModifyProfileImageRoutingModule {
}
