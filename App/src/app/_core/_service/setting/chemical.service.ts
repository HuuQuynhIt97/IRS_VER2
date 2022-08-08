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
export class ChemicalService extends CURDService<Chemical> {

  private bomSource = new BehaviorSubject({} as Chemical);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Chemical", utilitiesService);
  }
  changeHall(chemical: Chemical) {
    this.bomSource.next(chemical)
  }

 
}
