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
export class InkColorService extends CURDService<InkColor> {

  private bomSource = new BehaviorSubject({} as InkColor);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"InkColor", utilitiesService);
  }
  changeHall(shoeGlue: InkColor) {
    this.bomSource.next(shoeGlue)
  }
  
  loadData(colorGuid) {
    return this.http.get<any>(`${this.base}InkColor/LoadData?colorGuid=${colorGuid}`, {});
  }

}
