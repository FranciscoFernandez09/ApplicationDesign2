import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {AddDeviceToRoomComponent} from './add-device-to-room-page/add-device-to-room.component';


const routes: Routes = [
  {
    path: '',
    component: AddDeviceToRoomComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AddDeviceToRoomRoutingModule {
}
