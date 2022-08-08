import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { AccountRole } from '../_model/account-role';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class AccountRoleService extends CURDService<AccountRole> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AccountRole", utilitiesService);
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}AccountRole/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
