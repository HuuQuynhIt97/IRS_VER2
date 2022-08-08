import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { CodeType } from '../_model/code-type';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class CodeTypeService extends CURDService<CodeType> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"CodeType", utilitiesService);
  }

  getCodeTypesDropdownlist(): Observable<any> {
    return this.http.get<any>(`${this.base}CodeType/GetCodeTypesDropdownlist`, {});
  }
}
