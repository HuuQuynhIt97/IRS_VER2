import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { StockInChemical } from '../../_model/stock/stockInChemical';

@Injectable({
  providedIn: 'root'
})
export class StockInChemicalService extends CURDService<StockInChemical> {

  private bomSource = new BehaviorSubject({} as StockInChemical);
  currentInInk = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"StockInChemical", utilitiesService);
  }
  changeHall(stockInChemical: StockInChemical) {
    this.bomSource.next(stockInChemical)
  }
  updateExecution(model) {
    return this.http.post<any>(`${this.base}StockInChemical/UpdateExecution`, model);
  }
  approve(guid) {
    return this.http.post<any>(`${this.base}StockInChemical/Approve?guid=${guid}`, {});
  }
  unApprove(guid) {
    return this.http.post<any>(`${this.base}StockInChemical/UnApprove?guid=${guid}`, {});
  }

  dataFiterExecuteAndCreate(filter) {
    return this.http.post<any>(`${this.base}StockInChemical/DataFiterExecuteAndCreate`, filter);
  }
}
