import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { PigPedigree } from '../../_model/pigs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PigPedigreeService extends CURDService<PigPedigree> {

 private pigPedigreeSource = new BehaviorSubject({} );
  currentMakeOrder = this.pigPedigreeSource.asObservable();
  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"PigPedigree", utilitiesService);
  }
  changeMakeOrder(farm) {
    this.pigPedigreeSource.next(farm)
  }
}
