
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Pen, PenScreen } from '../../_model/farms';
import { OperationResult } from '../../_model/operation.result';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';


@Injectable({
  providedIn: 'root'
})
export class PenService extends CURDService<Pen> {
  private bomSource = new BehaviorSubject({} as PenScreen);
  currentPen = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Pen", utilitiesService);
  }
  changePen(farm: PenScreen) {
    this.bomSource.next(farm)
  }
  getPens(farmGuid) {
    return this.http.get<any>(`${this.base}Pen/GetPens?farmGuid=${farmGuid}`);
  }
  mapMakeOrderToPen(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/MapMakeOrderToPen`, model);
  }
  getPenByMakeOrderGuid(makeOrderGuid) {
    return this.http.get<any>(`${this.base}Pen/GetPenByMakeOrderGuid?makeOrderGuid=${makeOrderGuid}`);
  }
  getPenByRecord(recordGuid, type): Observable<any> {
    return this.http.get<any>(`${this.base}Pen/GetPenByRecord?recordGuid=${recordGuid}&type=${type}`, {});
  }
  getSelectedPen(guid: any[]): Observable<any> {
    let query = '';
    if (guid) {
      for (const item of guid) {
        query+=`guid=${item}&`
      }
    }
    return this.http.get<any>(`${this.base}Pen/GetSelectedPen?${query}`, {});
  }
  addRecord2Pen(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Pen/AddRecord2Pen`, model)
      .pipe(catchError(this.handleError));
  }

  removeRecord2Pen(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Pen/RemoveRecord2Pen`, model)
      .pipe(catchError(this.handleError));
  }
  getPensByRoomAndRecord(roomGuid, recordGuid, type): Observable<any> {
    const query = `roomGuid=${roomGuid}&recordGuid=${recordGuid}&type=${type}`
    return this.http.get<any>(`${this.base}Pen/getPensByRoomAndRecord?${query}`, {});
  }
}
