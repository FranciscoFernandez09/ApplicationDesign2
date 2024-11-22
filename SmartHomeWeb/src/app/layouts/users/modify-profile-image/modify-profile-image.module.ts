import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ModifyProfileImageComponent} from './modify-profile-image-page/modify-profile-image.component';
import {ModifyProfileImageRoutingModule} from './modify-profile-image-routing.module';
import {FormComponent} from '../../../components/form/form.component';
import {MainMenuComponent} from '../../../business-components/navmenus/main-menu/main-menu.component';
import {
  ServerResponseVisualization
} from '../../../business-components/server-response-visualization/server-response-visualization';

@NgModule({
  declarations: [ModifyProfileImageComponent],
  imports: [CommonModule, ModifyProfileImageRoutingModule, FormComponent, MainMenuComponent, ServerResponseVisualization],
})
export class ModifyProfileImageModule {
}
