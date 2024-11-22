import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {CreateCameraComponent} from './create-camera-page/create-camera.component';
import {CreateCameraRoutingModule} from './create-camera-routing.module';
import {FormComponent} from '../../../components/form/form.component';
import {MainMenuComponent} from "../../../business-components/navmenus/main-menu/main-menu.component";
import {
  ServerResponseVisualization
} from "../../../business-components/server-response-visualization/server-response-visualization";

@NgModule({
  declarations: [CreateCameraComponent],
  imports: [CommonModule, CreateCameraRoutingModule, FormComponent, MainMenuComponent, ServerResponseVisualization],
})
export class CreateCameraModule {
}
