import { WorkplanComponent } from './workplan/workplan.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExecutionRoutingModule } from './execution-routing.module';
import { ExecutionComponent } from './execution.component';
import { FormsModule } from '@angular/forms';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { Workplan2Component } from './workplan2/workplan2.component';


@NgModule({
  declarations: [
    ExecutionComponent,
    WorkplanComponent,
    Workplan2Component
  ],
  imports: [
    CommonModule,
    ExecutionRoutingModule,
    FormsModule,
    CoreDirectivesModule,
    SharedModule.forRoot(),
    Common2Module.forRoot()
  ]
})
export class ExecutionModule { }
