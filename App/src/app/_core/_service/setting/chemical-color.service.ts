import { ChemicalColor } from './../../_model/setting/chemicalColor';
import { InkColor } from './../../_model/setting/inkColor';
import { ShoeGlue } from './../../_model/setting/shoeGlue';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class ChemicalColorService extends CURDService<ChemicalColor> {

  private bomSource = new BehaviorSubject({} as ChemicalColor);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"ChemicalColor", utilitiesService);
  }
  changeHall(chemicalColor: ChemicalColor) {
    this.bomSource.next(chemicalColor)
  }
  
  loadData(colorGuid) {
    return this.http.get<any>(`${this.base}ChemicalColor/LoadData?colorGuid=${colorGuid}`, {});
  }

}
