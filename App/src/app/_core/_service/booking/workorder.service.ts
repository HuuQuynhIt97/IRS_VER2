import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { WorkOrder } from '../../_model/booking/workOrder';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class WorkorderService extends CURDService<WorkOrder>{

  private bomSource = new BehaviorSubject({} as WorkOrder);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"WorkOrder", utilitiesService);
  }
  changeHall(workOrder: WorkOrder) {
    this.bomSource.next(workOrder)
  }

  GetByHall(hallID) {
    return this.http.get<any>(`${this.base}WorkOrder/GetByHall?hallID=${hallID}`, {});
  }
}
