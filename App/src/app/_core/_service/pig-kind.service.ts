import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { PigKind } from '../_model/pig-kind';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class PigKindService extends CURDService<PigKind> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"PigKind", utilitiesService);
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}PigKind/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
