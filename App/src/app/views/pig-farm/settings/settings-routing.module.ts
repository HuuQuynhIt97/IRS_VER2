import { ProcessComponent } from './process/process.component';
import { PartComponent } from './part/part.component';
import { ColorComponent } from './color/color.component';
import { TreatmentWayComponent } from './treatment-way/treatment-way.component';
import { TreatmentComponent } from './treatment/treatment.component';
import { SupplierComponent } from './supplier/supplier.component';
import { InkComponent } from './Ink/Ink.component';
import { ShoeGlueComponent } from './shoe-glue/shoe-glue.component';
import { GlueChemicalComponent } from './glue-chemical/glue-chemical.component';
import { ShoeComponent } from './shoe/shoe.component';
import { GlueComponent } from './glue/glue.component';
import { ChemicalComponent } from './chemical/chemical.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { SettingsComponent } from './settings.component';

const routes: Routes = [
  { 
    path: '', 
    children: [
      {
        path: 'Chemicals',
        component: ChemicalComponent,
        data: {
          title: 'Chemicals',
          module: 'setting',
          breadcrumb: 'Chemicals',
          functionCode: 'Chemicals'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Supplier',
        component: SupplierComponent,
        data: {
          title: 'Supplier',
          module: 'setting',
          breadcrumb: 'Supplier',
          functionCode: 'Supplier'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Process',
        component: ProcessComponent,
        data: {
          title: 'Process',
          module: 'setting',
          breadcrumb: 'Process',
          functionCode: 'Process'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Color',
        component: ColorComponent,
        data: {
          title: 'Color',
          module: 'setting',
          breadcrumb: 'Color',
          functionCode: 'Color'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Part',
        component: PartComponent,
        data: {
          title: 'Part',
          module: 'setting',
          breadcrumb: 'Part',
          functionCode: 'Part'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Treatment',
        component: TreatmentComponent,
        data: {
          title: 'Treatment',
          module: 'setting',
          breadcrumb: 'Treatment',
          functionCode: 'Treatment'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Treatment-way',
        component: TreatmentWayComponent,
        data: {
          title: 'Treatment Way',
          module: 'setting',
          breadcrumb: 'Treatment Way',
          functionCode: 'Treatment-way'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Ink',
        component: InkComponent,
        data: {
          title: 'Ink',
          module: 'setting',
          breadcrumb: 'Ink',
          functionCode: 'Ink'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Glues',
        component: GlueComponent,
        data: {
          title: 'Glues',
          module: 'setting',
          breadcrumb: 'Glues',
          functionCode: 'Glues'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Shoe',
        component: ShoeComponent,
        data: {
          title: 'Shoe',
          module: 'setting',
          breadcrumb: 'Shoe',
          functionCode: 'Shoe'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'Glue-Chemical',
        component: GlueChemicalComponent,
        data: {
          title: 'Glue Chemical',
          module: 'setting',
          breadcrumb: 'Glue Chemical',
          functionCode: 'Glue-Chemical'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'Shoe-Glue',
        component: ShoeGlueComponent,
        data: {
          title: 'Shoe Glue',
          module: 'setting',
          breadcrumb: 'Shoe Glue',
          functionCode: 'Shoe-Glue'
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
export class SettingsRoutingModule { }
