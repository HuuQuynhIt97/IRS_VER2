import { RoomListSettingComponent } from './room-list-setting/room-list-setting.component';
import { ParentComponent } from './parent/parent.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClubManagementRoutingModule } from './club-management-routing.module';
import { HallComponent } from './hall/hall.component';
import { RoomComponent } from './room/room.component';
import { SiteComponent } from './site/site.component';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

import { DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { ImagesPipe } from 'src/app/_core/pipes/images.pipe';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { InfiniteScrollService } from '@syncfusion/ej2-angular-grids';
import { Filter, Page, Toolbar, TreeGrid } from '@syncfusion/ej2-angular-treegrid';
TreeGrid.Inject(Page, Toolbar, Filter);
const IMG_PIPE = [
  ImagesPipe
]
const ROUTING_MODULE = [
  ClubManagementRoutingModule
];
@NgModule({
  
  imports: [
    CommonModule,
    FormsModule,
    CoreDirectivesModule,
    ClubManagementRoutingModule,
    NgxDropzoneModule,
    SharedModule.forRoot(),
    Common2Module.forRoot(),
    ...ROUTING_MODULE
  ],
  declarations: [
    HallComponent,
    RoomComponent,
    SiteComponent,
    ParentComponent,
    RoomListSettingComponent
  ]
})
export class ClubManagementModule { 
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';

  constructor(config: NgbTooltipConfig
    ) {
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }

    }
}
