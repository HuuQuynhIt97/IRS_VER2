import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from 'src/app/_core/_guards/auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProfileComponent } from './profile/profile.component';
import { CustomerComponent } from './customer/customer.component';
import { SystemConfigComponent } from './system/system-config/system-config.component';
const routes: Routes = [
  {
    path: 'customer',
    component: CustomerComponent,
    data: {
      title: 'Customer',
      breadcrumb: 'Customer',
      functionCode: 'Customer'
    },
    canActivate: [AuthGuard]
  },
   {
    path: 'dashboard',
    component: DashboardComponent,
    data: {
      title: 'Dashboard',
      breadcrumb: 'Dashboard',
      functionCode: 'dashboard'
    },
   // canActivate: [AuthGuard]
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    data: {
      title: 'Dashboard',
      breadcrumb: 'Dashboard',
      functionCode: 'dashboard'
    },
    canActivate: [AuthGuard]
  },
  {
    path: 'dashboard/:guid',
    component: DashboardComponent,
    data: {
      title: 'Dashboard',
      breadcrumb: 'Dashboard',
      functionCode: 'dashboard'
    },
    canActivate: [AuthGuard]
  },

  {
    path: 'profile',
    component: ProfileComponent,
    data: {
      title: 'Profile',
      breadcrumb: 'Profile',
      functionCode: 'Profile'

    },
    canActivate: [AuthGuard]
  },
  {
    path: '',
    children: [
      {
        path: 'system-config',
        component: SystemConfigComponent,
        data: {
          title: 'System Config',
          breadcrumb: 'System Config',
          functionCode: 'System Config'

        },
        canActivate: [AuthGuard]
      },

      {
        path: 'system',
        data: {
          title: 'system',
          module: 'system',
          breadcrumb: 'System',
          root: true
        },
        loadChildren: () => import('./system/system.module').then(m => m.SystemModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'club-management',
        data: {
          title: 'Club-management',
          module: 'Club-management',
          breadcrumb: 'Club-management',
          root: true
        },
        loadChildren: () => import('./club-management/club-management.module').then(m => m.ClubManagementModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'report',
        data: {
          title: 'Report',
          module: 'Report',
          breadcrumb: 'Report',
          root: true
        },
        loadChildren: () => import('./report/report.module').then(m => m.ReportModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'booking',
        data: {
          title: 'booking',
          module: 'booking',
          breadcrumb: 'booking',
          root: true
        },
        loadChildren: () => import('./booking/booking.module').then(m => m.BookingModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'settings',
        data: {
          title: 'settings',
          module: 'settings',
          breadcrumb: 'settings',
          root: true
        },
        loadChildren: () => import('./settings/settings.module').then(m => m.SettingsModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'transaction',
        data: {
          title: 'transaction',
          module: 'transaction',
          breadcrumb: 'transaction',
          root: true
        },
        loadChildren: () => import('./transaction/transaction.module').then(m => m.TransactionModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'execution',
        data: {
          title: 'execution',
          module: 'execution',
          breadcrumb: 'execution',
          root: true
        },
        loadChildren: () => import('./execution/execution.module').then(m => m.ExecutionModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'stock',
        data: {
          title: 'stock',
          module: 'stock',
          breadcrumb: 'stock',
          root: true
        },
        loadChildren: () => import('./stock/stock.module').then(m => m.StockModule),
        //canActivate: [AuthGuard]
      },

      {
        path: 'farm',
        data: {
          title: 'Farm',
          module: 'Farm',
          breadcrumb: 'Farm',
          root: true
        },
        loadChildren: () => import('./farm/farm.module').then(m => m.FarmModule)
      },
      
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PigFarmRoutingModule { }
