import { CodePermissionComponent } from './code-permission/code-permission.component';
import { XAccountGroupComponent } from './xaccount-group/xaccount-group.component';
import { SystemMenuComponent } from './system-menu/system-menu.component';
import { SystemLanguageComponent } from './system-language/system-language.component';
import { EmployeeComponent } from './employee/employee.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { AccountComponent } from './account/account.component';
import { CodeTypeComponent } from './code-type/code-type.component';
import { SettingDashboardComponent } from './setting-dashboard/setting-dashboard.component';
import { ReportConfigComponent } from './report-config/report-config.component';
import { ReportChartConfigComponent } from './report-chart-config/report-chart-config.component';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'dashboard-setting',
        component: SettingDashboardComponent,
        data: {
          title: 'Dashboard Setting',
          module: 'system',
          breadcrumb: 'Dashboard Setting',
          functionCode: 'Dashboard Setting'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'code-type',
        component: CodeTypeComponent,
        data: {
          title: 'Code Type',
          module: 'system',
          breadcrumb: 'Code Type',
          functionCode: 'Code Type'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'code-permission',
        component: CodePermissionComponent,
        data: {
          title: 'Code Permission',
          module: 'system',
          breadcrumb: 'Code Permission',
          functionCode: 'Code Permission'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'report-config',
        component: ReportConfigComponent,
        data: {
          title: 'Report Config',
          module: 'system',
          breadcrumb: 'Report Config',
          functionCode: 'Report Config'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'report-chart-config',
        component: ReportChartConfigComponent,
        data: {
          title: 'Report Config - Chart',
          module: 'system',
          breadcrumb: 'Report Config - Chart',
          functionCode: 'Report Chart Config'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'system-language',
        component: SystemLanguageComponent,
        data: {
          title: 'System Language',
          module: 'system',
          breadcrumb: 'System Language',
          functionCode: 'System Language'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'employee',
        component: EmployeeComponent,
        data: {
          title: 'Employee',
          module: 'system',
          breadcrumb: 'Employee',
          functionCode: 'Employee'
        },
      canActivate: [AuthGuard]
      },
       {
        path: 'account-group',
        component: XAccountGroupComponent,
        data: {
          title: 'Account Group',
          module: 'system',
          breadcrumb: 'Account Group',
          functionCode: 'Account Group'
        },
       canActivate: [AuthGuard]
      },
       {
        path: 'account',
        component: AccountComponent,
        data: {
          title: 'Account',
          module: 'system',
          breadcrumb: 'Account',
          functionCode: 'Account'
        },
      canActivate: [AuthGuard]
      },
      {
       path: 'menu',
       component: SystemMenuComponent,
       data: {
         title: 'System Menu',
         module: 'Menu',
         breadcrumb: 'System Menu',
         functionCode: 'System Menu'
       },
       canActivate: [AuthGuard]
     }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemRoutingModule { }
