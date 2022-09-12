import { WorkplanComponent } from './workplan/workplan.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExecutionComponent } from './execution.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { Workplan2Component } from './workplan2/workplan2.component';

const routes: Routes = [
  {
    path: '',
    component: ExecutionComponent
  },
  {
    path: 'work-plan',
    component: WorkplanComponent,
    data: {
      title: 'Work Plan',
      module: 'execution',
      breadcrumb: 'Work Plan',
      functionCode: 'work-plan'
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'work-plan2',
    component: Workplan2Component,
    data: {
      title: 'Work Plan2',
      module: 'execution',
      breadcrumb: 'Work Plan2',
      functionCode: 'work-plan2'
    },
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExecutionRoutingModule { }
