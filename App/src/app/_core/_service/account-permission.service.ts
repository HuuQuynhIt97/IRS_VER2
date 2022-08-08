import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { AccountPermission } from '../_model/account-permission';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class AccountPermissionService extends CURDService<AccountPermission> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AccountPermission", utilitiesService);
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}AccountPermission/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
