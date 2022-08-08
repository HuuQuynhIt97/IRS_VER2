import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { SiteComponent } from 'src/app/views/pig-farm/club-management/site/site.component';
import { Site } from '../../_model/club-management/site';
import { Area, AreaScreen } from '../../_model/farms';
import { OperationResult } from '../../_model/operation.result';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class SiteService extends CURDService<Site> {

  private bomSource = new BehaviorSubject({} as Site);
  currentSite = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Site", utilitiesService);
  }
  changeSite(site: Site) {
    this.bomSource.next(site)
  }
  getSiteByAccount() {
    return this.http.get(`${this.base}Site/GetSitesByAccount`, {});
  }
  insertForm(model: Site): Observable<OperationResult> {
    for (const key in model) {
      if (Object.prototype.hasOwnProperty.call(model, key)) {
        let item = model[key];
        if (item instanceof Date) {
          model[key] = `${(item as Date).toLocaleDateString()} ${(item as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
    const file = model.file;
    delete model.file;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);
    return this.http.post<OperationResult>(`${this.base}Site/AddForm`, params).pipe(catchError(this.handleError));
  }

  updateForm(model: Site): Observable<OperationResult> {
    for (const key in model) {
      if (Object.prototype.hasOwnProperty.call(model, key)) {
        let item = model[key];
        if (item instanceof Date) {
          model[key] = `${(item as Date).toLocaleDateString()} ${(item as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
    const file = model.file;
    delete model.file;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);
    return this.http.post<OperationResult>(`${this.base}Site/updateForm`, params).pipe(catchError(this.handleError));
  }

}
