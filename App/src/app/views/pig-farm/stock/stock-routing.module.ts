import { StockInInkComponent } from './stock-in-ink/stock-in-ink.component';
import { StockInChemicalComponent } from './stock-in-chemical/stock-in-chemical.component';
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
    path: 'stock-In-Ink',
    component: StockInInkComponent,
    data: {
      title: 'Stock In Ink',
      module: 'stock',
      breadcrumb: 'Stock In Ink',
      functionCode: 'stock-in-ink'
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'stock-In-Chemical',
    component: StockInChemicalComponent,
    data: {
      title: 'Stock In chemical',
      module: 'stock',
      breadcrumb: 'Stock In chemical',
      functionCode: 'stock-in-chemical'
    },
    canActivate: [AuthGuard]
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
