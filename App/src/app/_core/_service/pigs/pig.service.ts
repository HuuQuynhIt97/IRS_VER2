import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Pig } from '../../_model/pigs/pig';
import { UtilitiesService } from '../utilities.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { OperationResult } from '../../_model/operation.result';
import { catchError } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class PigService extends CURDService<Pig> {
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Pig", utilitiesService);
  }
  getPigs( farmGuid): Observable<any> {
    return this.http.get<any>(`${this.base}Pig/getPigs?farmGuid=${farmGuid}`, {});
  }
  getPigByNo( no): Observable<any> {
    return this.http.get<any>(`${this.base}Pig/GetPigByNo?no=${no}`, {});
  }
  getFarms(): Observable<any> {
    return this.http.get<any>(`${this.base}Pig/getFarms`, {});
  }
  getAreas(farmGuid): Observable<any> {
    const query = `farmGuid=${farmGuid}`
    return this.http.get<any>(`${this.base}Pig/getAreas?${query}`, {});
  }
  getBarns(farmGuid, areaGuid): Observable<any> {
    const query = `farmGuid=${farmGuid}&areaGuid=${areaGuid}`
    return this.http.get<any>(`${this.base}Pig/getBarns?${query}`, {});
  }
  getCullingTanks(farmGuid, areaGuid): Observable<any> {
    const query = `farmGuid=${farmGuid}&areaGuid=${areaGuid}`
    return this.http.get<any>(`${this.base}Pig/getCullingTanks?${query}`, {});
  }
  getRooms(farmGuid, areaGuid, barnGuid): Observable<any> {
    const query = `farmGuid=${farmGuid}&areaGuid=${areaGuid}&barnGuid=${barnGuid}`
    return this.http.get<any>(`${this.base}Pig/getRooms?${query}`, {});
  }
  getPens(farmGuid, areaGuid, barnGuid, roomGuid): Observable<any> {
    const query = `farmGuid=${farmGuid}&areaGuid=${areaGuid}&barnGuid=${barnGuid}&roomGuid=${roomGuid}`
    return this.http.get<any>(`${this.base}Pig/getPens?${query}`, {});
  }
  getPigsByPenAndRecord(penGuid, recordGuid, type): Observable<any> {
    const query = `penGuid=${penGuid}&recordGuid=${recordGuid}&type=${type}`
    return this.http.get<any>(`${this.base}Pig/GetPigsByPenAndRecord?${query}`, {});
  }
  addRecord2Pig(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Record/AddRecord2Pen`, model)
      .pipe(catchError(this.handleError));
  }

  removeRecord2Pig(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Record/RemoveRecord2Pen`, model)
      .pipe(catchError(this.handleError));
  }
  getSelectedPig(guid: any[]): Observable<any> {
    let query = '';
    if (guid) {
      for (const item of guid) {
        query+=`guid=${item}&`
      }

    }
    return this.http.get<any>(`${this.base}Pig/GetSelectedPig?${query}`, {});
  }
  getSelectedPig2(guid: any[], recordGuid: string, type: string): Observable<any> {
    let query = '';
    if (guid) {
      for (const item of guid) {
        query+=`guid=${item}&`
      }

    }
    return this.http.get<any>(`${this.base}Pig/GetSelectedPig2?${query}recordGuid=${recordGuid}&type=${type}`, {});
  }
  getSelectedPigsByRecord(recordGuid, type): Observable<any[]> {
    const query = `recordGuid=${recordGuid}&type=${type}`
    return this.http.get<any[]>(`${this.base}Pig/GetSelectedPigsByRecord?${query}`, {});
  }
}
