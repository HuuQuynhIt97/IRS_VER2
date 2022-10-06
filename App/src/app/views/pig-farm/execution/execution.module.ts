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
import { ColorMixingWorkPlanComponent } from './color-mixing-work-plan/color-mixing-work-plan.component';
import { NgbModule, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    ExecutionComponent,
    WorkplanComponent,
    Workplan2Component,
    ColorMixingWorkPlanComponent
  ],
  imports: [
    CommonModule,
    ExecutionRoutingModule,
    FormsModule,
    NgbModule,
    CoreDirectivesModule,
    SharedModule.forRoot(),
    Common2Module.forRoot()
  ]
})
export class ExecutionModule {

  constructor(config: NgbTooltipConfig
    ) {
      config.disableTooltip = true;

    }
}

// export class SystemModule {
//   isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
//   constructor(config: NgbTooltipConfig
//     ) {
//       if (this.isAdmin === false) {
//         config.disableTooltip = true;
//       }

//     }
// }
