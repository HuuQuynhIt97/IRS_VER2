import { ShoeGlue } from './../../_model/setting/shoeGlue';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class ShoeGlueService extends CURDService<ShoeGlue> {

  private bomSource = new BehaviorSubject({} as ShoeGlue);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"ShoeGlue", utilitiesService);
  }
  changeHall(shoeGlue: ShoeGlue) {
    this.bomSource.next(shoeGlue)
  }
  
  loadData(shoeGuid) {
    return this.http.get<any>(`${this.base}ShoeGlue/LoadData?shoeGuid=${shoeGuid}`, {});
  }

  getMenuPageSetting() {
    return this.http.get<any>(`${this.base}ShoeGlue/getMenuPageSetting`, {});
  }

  getRecipePageSetting() {
    return this.http.get<any>(`${this.base}ShoeGlue/getRecipePageSetting`, {});
  }
}
