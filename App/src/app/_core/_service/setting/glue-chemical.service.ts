import { GlueChemical } from './../../_model/setting/glueChemical';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { WorkOrder } from '../../_model/booking/workOrder';
import { Chemical } from '../../_model/setting/chemical';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class GlueChemicalService extends CURDService<GlueChemical> {

  private bomSource = new BehaviorSubject({} as GlueChemical);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"GlueChemical", utilitiesService);
  }
  changeHall(glueChemical: GlueChemical) {
    this.bomSource.next(glueChemical)
  }
  
  loadData(glueGuid) {
    return this.http.get<any>(`${this.base}GlueChemical/LoadData?glueGuid=${glueGuid}`, {});
  }
}
