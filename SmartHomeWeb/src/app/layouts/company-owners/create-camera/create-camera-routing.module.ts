import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateCameraComponent} from './create-camera-page/create-camera.component';

const routes: Routes = [
  {
    path: '',
    component: CreateCameraComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CreateCameraRoutingModule {
}
