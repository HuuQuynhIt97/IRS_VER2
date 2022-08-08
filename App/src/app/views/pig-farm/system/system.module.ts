import { CodeTypeComponent } from './code-type/code-type.component';
import { CodePermissionComponent } from './code-permission/code-permission.component';
import { SystemLanguageComponent } from './system-language/system-language.component';
import { EmployeeComponent } from './employee/employee.component';
import { AccountComponent } from './account/account.component';
// Angular
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
// Components Routing

import { DropDownListModule, MultiSelectModule } from '@syncfusion/ej2-angular-dropdowns';
import { NgbModule, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
// Import ngx-barcode module
import { SwitchModule, RadioButtonModule, CheckBoxAllModule } from '@syncfusion/ej2-angular-buttons';

import { ButtonModule } from '@syncfusion/ej2-angular-buttons';

import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';

import { HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { DatePipe } from '@angular/common';

import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { ColorPickerModule } from '@syncfusion/ej2-angular-inputs';

import { ToolbarModule, TreeViewAllModule } from '@syncfusion/ej2-angular-navigations';
import { TreeGridAllModule } from '@syncfusion/ej2-angular-treegrid/';
import { TreeGrid, Page, Toolbar, Filter } from '@syncfusion/ej2-treegrid';
import { CountdownModule } from 'ngx-countdown';
import { CustomLoader } from 'src/app/_core/_helper/custom-loader';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { SystemRoutingModule } from './system.routing.module';
import { ImagesPipe } from 'src/app/_core/pipes/images.pipe';
import { SystemMenuComponent } from './system-menu/system-menu.component';
import { XAccountGroupComponent } from './xaccount-group/xaccount-group.component';
import { SystemComponent } from './system.component';
import { PrivilegeComponent } from './xaccount-group/privilege/privilege.component';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { SettingDashboardComponent } from './setting-dashboard/setting-dashboard.component';
import { SettingDashboardNormalComponent } from './setting-dashboard/setting-dashboard-normal/setting-dashboard-normal.component';
import { SettingDashboardChartComponent } from './setting-dashboard/setting-dashboard-chart/setting-dashboard-chart.component';
import { SettingDashboardTableComponent } from './setting-dashboard/setting-dashboard-table/setting-dashboard-table.component';
import { CreateDashboardComponent } from './setting-dashboard/create-dashboard/create-dashboard.component';
import { CreateAreaComponent } from './setting-dashboard/create-area/create-area.component';
declare var require: any;

TreeGrid.Inject(Page, Toolbar, Filter);

const IMG_PIPE = [
  ImagesPipe,
]
@NgModule({
  providers: [
    DatePipe,
    NgbTooltipConfig
  ],
  imports: [
    ButtonModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgbModule,
    CountdownModule,
    CoreDirectivesModule,
    SystemRoutingModule,
    ColorPickerModule,
    SharedModule.forRoot(),
    Common2Module.forRoot(),
  ],
  declarations: [
    ...IMG_PIPE,
    AccountComponent,
    PrivilegeComponent,
    SystemComponent,
    XAccountGroupComponent,
    EmployeeComponent,
    SystemLanguageComponent,
    SystemMenuComponent,
    CodePermissionComponent,
    CodeTypeComponent,
    SettingDashboardComponent,
    SettingDashboardChartComponent,
    SettingDashboardTableComponent,
    SettingDashboardNormalComponent,
    CreateDashboardComponent,
    CreateAreaComponent,
  ]
})
export class SystemModule {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  constructor(config: NgbTooltipConfig
    ) {
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }

    }
}
