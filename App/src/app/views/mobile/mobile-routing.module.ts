import { BookingHistoryComponent } from './booking-history/booking-history.component';
import { BookingComponent } from './booking/booking.component';
import { SearchByListComponent } from './search-by-list/search-by-list.component';
import { BookingCalendarScreenComponent } from './booking-calendar-screen/booking-calendar-screen.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { MobileComponent } from './mobile.component';
import { HomeComponent } from './home/home.component';
import { HallDetailComponent } from './hall-detail/hall-detail.component';
const routes: Routes = [
  { 
    path: '',
    component: LayoutComponent,
    runGuardsAndResolvers: 'always',
    children: [
      {
        path: 'home',
        component: HomeComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
      {
        path: 'booking-calendar-screen',
        component: BookingCalendarScreenComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
      {
        path: 'search-by-list',
        component: SearchByListComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
      {
        path: 'hall-detail/:roomID/:bookingID',
        component: HallDetailComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
      {
        path: 'booking/:id',
        component: BookingComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
      {
        path: 'booking-history',
        component: BookingHistoryComponent,
        data: {
          title: 'home',
          breadcrumb: 'Home',
          functionCode: 'Home Mobile'

        },
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MobileRoutingModule { 

}
