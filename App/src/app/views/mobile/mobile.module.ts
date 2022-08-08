import { loadCldr, setCulture } from '@syncfusion/ej2-base';
import { BookingCalendarScreenComponent } from './booking-calendar-screen/booking-calendar-screen.component';
import { HomeComponent } from './home/home.component';
import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';

import { MobileRoutingModule } from './mobile-routing.module';
import { MobileComponent } from './mobile.component';
import { LayoutComponent } from './layout/layout.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { DatePickerAllModule } from '@syncfusion/ej2-angular-calendars';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { ChartModule } from '@syncfusion/ej2-angular-charts';
import { MenuAllModule, SidebarModule, TreeViewAllModule } from '@syncfusion/ej2-angular-navigations';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';
import { SearchByListComponent } from './search-by-list/search-by-list.component';
import { HallDetailComponent } from './hall-detail/hall-detail.component';
import { BookingComponent } from './booking/booking.component';
import { BookingHistoryComponent } from './booking-history/booking-history.component';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
let lang = localStorage.getItem('lang');
if (!lang) {
  localStorage.setItem('lang', 'tw');
  lang = localStorage.getItem('lang');
}
setCulture(lang);
declare var require: any;
loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/en/ca-gregorian.json'),
  require('cldr-data/main/en/numbers.json'),
  require('cldr-data/main/en/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 


// loadCldr(
//   require('cldr-data/supplemental/numberingSystems.json'),
//   require('cldr-data/main/vi/ca-gregorian.json'),
//   require('cldr-data/main/vi/numbers.json'),
//   require('cldr-data/main/vi/timeZoneNames.json'),
//   require('cldr-data/supplemental/weekdata.json')); 

loadCldr(
  require('cldr-data/supplemental/numberingSystems.json'),
  require('cldr-data/main/zh/ca-gregorian.json'),
  require('cldr-data/main/zh/numbers.json'),
  require('cldr-data/main/zh/timeZoneNames.json'),
  require('cldr-data/supplemental/weekdata.json')); 
  
@NgModule({
  declarations: [
    MobileComponent,
    LayoutComponent,
    HomeComponent,
    BookingCalendarScreenComponent,
    SearchByListComponent,
    HallDetailComponent,
    BookingComponent,
    BookingHistoryComponent
  ],
  imports: [
    CommonModule,
    NgxGalleryModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    MobileRoutingModule,
    NgbModule,
    DatePickerAllModule,
    CoreDirectivesModule,
    ChartModule,
    SidebarModule,
    MenuAllModule,
    TreeViewAllModule,
    SharedModule.forRoot(),
    Common2Module.forRoot(),
  ],
  providers: [
    DatePipe
  ]
})
export class MobileModule { }
