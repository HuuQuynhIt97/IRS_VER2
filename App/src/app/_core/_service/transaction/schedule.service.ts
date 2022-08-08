import { Schedule } from './../../_model/transaction/schedule';
import { ShoeGlue } from './../../_model/setting/shoeGlue';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService extends CURDService<Schedule> {

  private bomSource = new BehaviorSubject({} as Schedule);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Schedule", utilitiesService);
  }
  changeHall(schedule: Schedule) {
    this.bomSource.next(schedule)
  }
  
  loadData(shoeGuid) {
    return this.http.get<any>(`${this.base}Schedule/LoadData?shoeGuid=${shoeGuid}`, {});
  }


}
