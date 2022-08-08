import { RoomListSettingComponent } from './room-list-setting/room-list-setting.component';
import { HallComponent } from './hall/hall.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { ParentComponent } from './parent/parent.component';
import { RoomComponent } from './room/room.component';
// import { RoomComponent } from './room/room.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'Hall_List',
        component: RoomListSettingComponent,
        data: {
          title: 'Hall Data Setting',
          module: 'club-management',
          breadcrumb: 'Hall Data Setting',
          functionCode: 'Hall'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'room',
        component: ParentComponent,
        data: {
          title: 'Room Data Setting',
          module: 'club-management',
          breadcrumb: 'Room Data Setting',
          functionCode: 'Room'
        },
        canActivate: [AuthGuard]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClubManagementRoutingModule { }
