import { InInk } from './../../_model/stock/inInk';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class InInkService extends CURDService<InInk> {
  private bomSource = new BehaviorSubject({} as InInk);
  currentInInk = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"InInk", utilitiesService);
  }
  changeHall(inInk: InInk) {
    this.bomSource.next(inInk)
  }
  
  loadData() {
    return this.http.get<any>(`${this.base}InInk/LoadData`, {});
  }
  outOfStock(inInkGuid) {
    return this.http.post<any>(`${this.base}InInk/OutOfStock?inInkGuid=${inInkGuid}`, {});
  }

}
