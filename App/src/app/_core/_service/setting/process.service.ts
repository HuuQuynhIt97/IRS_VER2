import { Process } from './../../_model/setting/process';
import { Ink } from './../../_model/setting/ink';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Chemical } from '../../_model/setting/chemical';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { Supplier } from '../../_model/setting/supplier';

@Injectable({
  providedIn: 'root'
})
export class ProcessService extends CURDService<Process> {

  private bomSource = new BehaviorSubject({} as Process);
  currentProcess = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Process", utilitiesService);
  }
  changeSupplier(process: Process) {
    this.bomSource.next(process)
  }
  

}
