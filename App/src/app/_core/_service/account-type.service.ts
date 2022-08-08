import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { AccountType } from '../_model/account-type';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class AccountTypeService extends CURDService<AccountType> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AccountType", utilitiesService);
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}AccountType/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
