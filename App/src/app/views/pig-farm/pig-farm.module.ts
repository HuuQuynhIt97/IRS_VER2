import { SharedModule } from 'src/app/_core/commons/shared.module';
import { CustomerComponent } from './customer/customer.component';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
// Angular
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
// Components Routing
import { PigFarmRoutingModule } from './pig-farm-routing.module';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NgbModule, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { ChartModule } from '@syncfusion/ej2-angular-charts';
import { Container1Component } from './dashboard/container1/container1.component';
import { Container2Component } from './dashboard/container2/container2.component';
import { Container3Component } from './dashboard/container3/container3.component';
import { DashboardContainerComponent } from './dashboard/dashboard-container/dashboard-container.component';
import { SystemConfigComponent } from './system/system-config/system-config.component';

const PHASE4_COMPONENT = [
  CustomerComponent,
  SystemConfigComponent
]

@NgModule({
  providers: [
    DatePipe,
    NgbTooltipConfig
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PigFarmRoutingModule,
    NgbModule,
    DatePickerAllModule,
    CoreDirectivesModule,
    ChartModule,
    SharedModule.forRoot(),
    Common2Module.forRoot(),
  ],
  declarations: [
    DashboardComponent,
    Container1Component,
    Container2Component,
    Container3Component,
    DashboardContainerComponent,
    ...PHASE4_COMPONENT,
  ]
})
export class PigFarmModule {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';

  constructor(config: NgbTooltipConfig
    ) {
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }

    }
 }
