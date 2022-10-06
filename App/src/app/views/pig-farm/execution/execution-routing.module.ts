import { WorkplanComponent } from './workplan/workplan.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExecutionComponent } from './execution.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { Workplan2Component } from './workplan2/workplan2.component';
import { ColorMixingWorkPlanComponent } from './color-mixing-work-plan/color-mixing-work-plan.component';

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
      title: 'Buying List',
      module: 'execution',
      breadcrumb: 'Buying List',
      functionCode: 'buying-list'
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'color-mixing-work-plan',
    component: ColorMixingWorkPlanComponent,
    data: {
      title: 'Color Mixing Work Plan',
      module: 'execution',
      breadcrumb: 'Color Mixing Work Plan',
      functionCode: 'color-mixing-wp'
    },
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExecutionRoutingModule { }
