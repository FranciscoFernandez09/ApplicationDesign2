import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {WelcomeRoutingModule} from './welcome-routing.module';
import {WelcomeComponent} from './welcome-page/welcome.component';
import {MainMenuComponent} from '../../business-components/navmenus/main-menu/main-menu.component';

@NgModule({
  declarations: [WelcomeComponent],
  imports: [CommonModule, WelcomeRoutingModule, MainMenuComponent],
})
export class WelcomeModule {
}
