import { Ink } from './../../_model/setting/ink';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Chemical } from '../../_model/setting/chemical';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class InkService extends CURDService<Ink> {

  private bomSource = new BehaviorSubject({} as Ink);
  currentInk = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Ink", utilitiesService);
  }
  changeInk(ink: Ink) {
    this.bomSource.next(ink)
  }


}
