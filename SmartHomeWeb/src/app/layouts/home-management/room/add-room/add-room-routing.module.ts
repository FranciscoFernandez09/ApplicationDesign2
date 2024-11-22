import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AddRoomComponent} from './add-room-page/add-room.component';

const routes: Routes = [
  {
    path: '',
    component: AddRoomComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AddRoomRoutingModule {
}
