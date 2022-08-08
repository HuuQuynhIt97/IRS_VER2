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
export class SupplierService extends CURDService<Supplier> {

  private bomSource = new BehaviorSubject({} as Supplier);
  currentInk = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Supplier", utilitiesService);
  }
  changeSupplier(supplier: Supplier) {
    this.bomSource.next(supplier)
  }

  getAllSupplierByTreatment(id) {
    return this.http.get<Supplier[]>(this.base + `Supplier/GetAllByTreatment?ID=${id}`, {});
  }

}
