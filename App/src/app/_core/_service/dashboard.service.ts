import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Dashboard } from '../_model/dashboard';
import { UtilitiesService } from './utilities.service';
import { BehaviorSubject, Observable, ReplaySubject } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class DashboardService extends CURDService<Dashboard> {
    private areaSource = new BehaviorSubject({} as string);
    currentArea = this.areaSource.asObservable();

    changeAreaGuid(areaGuid) {
      this.areaSource.next(areaGuid)
    }

    private dashboardSource = new BehaviorSubject({} as string);
    currentDashboard = this.dashboardSource.asObservable();

    changeDashboardGuid(dashboardGuid) {
      this.dashboardSource.next(dashboardGuid)
    }

    private farmSource = new ReplaySubject(1);
    currentFarm = this.farmSource.asObservable();

    changeFarmGuid(farmGuid) {
      this.farmSource.next(farmGuid)
    }
  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Dashboard", utilitiesService);
  }

  getDashboardsDropdownlist(): Observable<any> {
    return this.http.get<any>(`${this.base}Dashboard/GetDashboardsDropdownlist`, {});
  }
  getDashboards(dashboardGuid, yearMonth, lang): Observable<any> {
    return this.http.get<any>(`${this.base}Dashboard/GetDashboards?dashboardGuid=${dashboardGuid}&yearMonth=${yearMonth}&lang=${lang}`, {});
  }
  getDashboardsNav(farmGuid): Observable<any> {
    return this.http.get<any>(`${this.base}Dashboard/GetDashboardsNav?farmGuid=${farmGuid}`, {});
  }
}
