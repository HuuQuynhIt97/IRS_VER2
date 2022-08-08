import { Hall } from './../../_model/club-management/hall';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Site } from '../../_model/club-management/site';
import { OperationResult } from '../../_model/operation.result';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
@Injectable({
  providedIn: 'root'
})
export class HallService extends CURDService<Hall>{

  private bomSource = new BehaviorSubject({} as Hall);
  currentHall = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Hall", utilitiesService);
  }
  changeHall(hall: Hall) {
    this.bomSource.next(hall)
  }

  loadDataBySiteGuiId(siteID) {
    return this.http.get<any>(`${this.base}Hall/LoadDataBySite?siteID=${siteID}`, {});
  }

  

}
