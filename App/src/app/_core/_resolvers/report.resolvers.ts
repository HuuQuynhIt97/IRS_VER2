import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { AlertifyService } from '../_service/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Oc } from '../_model/oc';
import { ReportService } from '../_service/report.service';

@Injectable()
export class ReportResolver implements Resolve<object> {
  constructor(
    private service: ReportService,
    private router: Router,
    private route: ActivatedRoute,
    private alertify: AlertifyService
  ) {
  }

  resolve(route: ActivatedRouteSnapshot): Observable<object> {
    const kind = route.paramMap.get('kind');
    const menuLink = `/report/${kind}`;
    return this.service.getReportType(menuLink).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        this.router.navigate(['/login']);
        return of(null);
      })
    );
  }
}
