import { InChemicalComponent } from './in-chemical/in-chemical.component';
import { InInkComponent } from './in-ink/in-ink.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StockComponent } from './stock.component';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';

const routes: Routes = [
  { 
    path: '', 
    component: StockComponent 
  },
  {
    path: 'In-Ink',
    component: InInkComponent,
    data: {
      title: 'In Ink',
      module: 'stock',
      breadcrumb: 'In Ink',
      functionCode: 'In-Ink'
    },
    canActivate: [AuthGuard]
  },

  {
    path: 'In-Chemical',
    component: InChemicalComponent,
    data: {
      title: 'In Chemical',
      module: 'stock',
      breadcrumb: 'In Chemical',
      functionCode: 'In-Chemical'
    },
    canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StockRoutingModule { }
