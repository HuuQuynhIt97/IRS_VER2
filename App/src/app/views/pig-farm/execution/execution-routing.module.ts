import { WorkplanComponent } from './workplan/workplan.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExecutionComponent } from './execution.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';

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
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExecutionRoutingModule { }
