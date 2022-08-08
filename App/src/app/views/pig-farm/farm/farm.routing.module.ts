import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthGuard } from "src/app/_core/_guards/auth.guard";
import { FarmParentComponent } from "./farm-parent/farm-parent.component";
import { FarmComponent } from "./farm/farm.component";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'setting',
        component: FarmParentComponent,
        data: {
          title: 'Farm Setting',
          module: 'Farm Setting',
          breadcrumb: 'Farm',
          functionCode: 'Farm'
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
export class FarmRoutingModule { }
