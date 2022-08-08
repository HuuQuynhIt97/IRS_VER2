import { Process2 } from './../../_model/setting/process2';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class Process2Service extends CURDService<Process2> {

  private colorSource = new BehaviorSubject({} as Process2);
  currentProcess2 = this.colorSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Process2", utilitiesService);
  }
  changeColor(process2: Process2) {
    this.colorSource.next(process2)
  }

}
