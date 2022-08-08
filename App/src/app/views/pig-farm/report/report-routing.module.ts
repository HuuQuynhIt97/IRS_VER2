import { Report2Component } from './report2/report2.component';
import { Report1Component } from './report1/report1.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportComponent } from './report.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { Report3Component } from './report3/report3.component';
import { Report4Component } from './report4/report4.component';

const routes: Routes = [
  { 
    path: '', 
    children: [
      {
        path: 'A1',
        component: Report1Component,
        data: {
          title: 'Report 1',
          module: 'report',
          breadcrumb: 'Report 1',
          functionCode: 'Report1'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'A2',
        component: Report2Component,
        data: {
          title: 'Report 2',
          module: 'report',
          breadcrumb: 'Report 2',
          functionCode: 'Report2'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'A3',
        component: Report3Component,
        data: {
          title: 'Report 3',
          module: 'report',
          breadcrumb: 'Report 3',
          functionCode: 'Report3'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'A4',
        component: Report4Component,
        data: {
          title: 'Report 4',
          module: 'report',
          breadcrumb: 'Report 4',
          functionCode: 'Report4'
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
export class ReportRoutingModule { }
