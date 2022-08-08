import { InChemical } from './../../_model/stock/inChemical';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class InChemicalService extends CURDService<InChemical> {
  private bomSource = new BehaviorSubject({} as InChemical);
  currentInChemical = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"InChemical", utilitiesService);
  }
  changeHall(inChemical: InChemical) {
    this.bomSource.next(inChemical)
  }
  
  loadData() {
    return this.http.get<any>(`${this.base}InChemical/LoadData`, {});
  }

  outOfStock(inChemicalGuid) {
    return this.http.post<any>(`${this.base}InChemical/OutOfStock?inChemicalGuid=${inChemicalGuid}`, {});
  }

}