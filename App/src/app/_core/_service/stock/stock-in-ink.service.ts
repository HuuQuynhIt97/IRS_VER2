import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { StockInInk } from '../../_model/stock/stockInInk';

@Injectable({
  providedIn: 'root'
})
export class StockInInkService extends CURDService<StockInInk> {

  private bomSource = new BehaviorSubject({} as StockInInk);
  currentInInk = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"StockInInk", utilitiesService);
  }
  changeHall(stockInInk: StockInInk) {
    this.bomSource.next(stockInInk)
  }
  
  updateExecution(model) {
    return this.http.post<any>(`${this.base}StockInInk/UpdateExecution`, model);
  }
  approve(guid) {
    return this.http.post<any>(`${this.base}StockInInk/Approve?guid=${guid}`, {});
  }
  unApprove(guid) {
    return this.http.post<any>(`${this.base}StockInInk/UnApprove?guid=${guid}`, {});
  }

  dataFiterExecuteAndCreate(filter) {
    return this.http.post<any>(`${this.base}StockInInk/DataFiterExecuteAndCreate`, filter);
  }


}
