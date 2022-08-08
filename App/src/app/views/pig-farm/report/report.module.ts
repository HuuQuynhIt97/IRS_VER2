import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportRoutingModule } from './report-routing.module';
import { ReportComponent } from './report.component';
import { Report1Component } from './report1/report1.component';
import { Report2Component } from './report2/report2.component';
import { Report3Component } from './report3/report3.component';
import { Report4Component } from './report4/report4.component';


@NgModule({
  declarations: [
    ReportComponent,
    Report1Component,
    Report2Component,
    Report3Component,
    Report4Component
  ],
  imports: [
    CommonModule,
    ReportRoutingModule
  ]
})
export class ReportModule { }
