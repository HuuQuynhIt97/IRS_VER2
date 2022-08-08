import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CURDService } from './base/CURD.service';
import { UtilitiesService } from './utilities.service';
import { Customer } from '../_model/customer';
@Injectable({
  providedIn: 'root'
})
export class CustomerService extends CURDService<Customer> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Customer", utilitiesService);
  }
  getCustomers(farmGuid) {
    return this.http.get<any>(`${this.base}Customer/GetCustomers?farmGuid=${farmGuid}`, {});
  }
}
