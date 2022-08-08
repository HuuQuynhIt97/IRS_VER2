import { LYWorkOrderComponent } from './LY-WorkOrder/LY-WorkOrder.component';
import { BookingCheckComponent } from './booking-check/booking-check.component';
import { BookingCalendarComponent } from './booking-calendar/booking-calendar.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BookingRoutingModule } from './booking-routing.module';
import { FormsModule } from '@angular/forms';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { ScheduleModule } from '@syncfusion/ej2-angular-schedule';
import { L10n, loadCldr, setCulture } from '@syncfusion/ej2-base';
import { DayService, WeekService, WorkWeekService, MonthService, AgendaService, MonthAgendaService} from '@syncfusion/ej2-angular-schedule';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';
declare var require: any;
let lang = localStorage.getItem('lang');
let languages = JSON.parse(localStorage.getItem('languages'));
setCulture(lang);
let load = {
  [lang]: {
    grid: languages['grid'],
    pager: languages['pager'],
    dropdowns: languages['dropdownlist'],
    schedule: languages['schedule']
  }
};
L10n.load(load);

loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 


loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/vi/ca-gregorian.json'),
  require('cldr-data/main/vi/numbers.json'),
  require('cldr-data/main/vi/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 


loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/zh/ca-gregorian.json'),
  require('cldr-data/main/zh/numbers.json'),
  require('cldr-data/main/zh/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 
// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
  
@NgModule({
  declarations: [
    BookingCalendarComponent,
    BookingCheckComponent,
    LYWorkOrderComponent,
  ],
  imports: [
    ScheduleModule,
    CommonModule,
    FormsModule,
    CoreDirectivesModule,
    SharedModule.forRoot(),
    Common2Module.forRoot(),
    BookingRoutingModule
  ],
  providers: [
    DayService, 
    WeekService, 
    WorkWeekService, 
    MonthService,
    AgendaService,
    MonthAgendaService
  ]
})
export class BookingModule { }
