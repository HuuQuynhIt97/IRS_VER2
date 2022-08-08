import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Farm, FarmScreen } from '../../_model/farms';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';


@Injectable({
  providedIn: 'root'
})
export class FarmService extends CURDService<Farm> {
  private bomSource = new BehaviorSubject({} as FarmScreen);
  currentFarm = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Farm", utilitiesService);
  }
  changeFarm(farm: FarmScreen) {
    this.bomSource.next(farm)
  }
  getFarmsByAccount() {
    return this.http.get(`${this.base}Farm/GetFarmsByAccount`, {});
  }
  getFarms() {
    return this.http.get(`${this.base}Farm/GetFarms`, {});
  }
}
