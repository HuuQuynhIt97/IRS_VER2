import { NameRecipeWorkerComponent } from './name-recipe-worker/name-recipe-worker.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TransactionComponent } from './transaction.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: TransactionComponent
  },
  {
    path: 'Schedule',
    component: NameRecipeWorkerComponent,
    data: {
      title: 'Receipt',
      module: 'transaction',
      breadcrumb: 'Receipt',
      functionCode: 'Receipt'
    },
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TransactionRoutingModule { }
