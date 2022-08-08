import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Method } from '../_model/method';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class MethodService extends CURDService<Method> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Method", utilitiesService);
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}Method/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
