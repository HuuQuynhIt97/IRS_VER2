import { LYWorkOrderComponent } from './LY-WorkOrder/LY-WorkOrder.component';
import { BookingCheckComponent } from './booking-check/booking-check.component';
import { BookingCalendarComponent } from './booking-calendar/booking-calendar.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'Booking_Calendar',
        component: BookingCalendarComponent,
        data: {
          title: 'Booking Calendar',
          module: 'booking',
          breadcrumb: 'Booking',
          functionCode: 'Booking-Calendar'
        },
        canActivate: [AuthGuard]
      },
      {
        path: 'Booking_Check',
        component: BookingCheckComponent,
        data: {
          title: 'Booking Check',
          module: 'booking',
          breadcrumb: 'Booking Detail',
          functionCode: 'Booking_Check'
        },
        canActivate: [AuthGuard]
      },

      {
        path: 'LY_WorkOrder',
        component: LYWorkOrderComponent,
        data: {
          title: 'Booking Ly WorkOrder',
          module: 'booking',
          breadcrumb: 'Booking WorkOrder',
          functionCode: 'ly_workOrder'
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
export class BookingRoutingModule { }
