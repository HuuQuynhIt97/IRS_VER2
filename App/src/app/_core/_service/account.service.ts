import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
import { Account } from '../_model/account';
@Injectable({
  providedIn: 'root'
})
export class AccountService extends CURDService<Account> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Account", utilitiesService);
  }
  showPassword(key): Observable<any> {
    const params = new FormData();
    params.append("key", key);
    return this.http.post<any>(`${this.base}Account/ShowPassword`, params).pipe(catchError(this.handleError));
  }
  changePassword(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.base}Account/changePassword`, model).pipe(catchError(this.handleError));
  }
  insertForm(model: Account): Observable<OperationResult> {
    for (const audit of this.audits) {
      let value2 = model[audit];
      if (value2 instanceof Date) {
        model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
      }
    }
    if (model.ocidList.length === 0) {
      delete model.ocidList
    }
    const file = model.file;
    const ocidList = model.ocidList;
    delete model.file;
    delete model.ocidList;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);
    let query = '';
    if (ocidList) {
      for (const item of ocidList) {
        query+=`ocidList=${item}&`
      }
    }
    return this.http.post<OperationResult>(`${this.base}Account/AddForm?${query}`, params).pipe(catchError(this.handleError));
  }
  updateForm(model: Account): Observable<OperationResult> {
    for (const audit of this.audits) {
      let value2 = model[audit];
      if (value2 instanceof Date) {
        model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
      }
    }
    if (model.ocidList.length === 0) {
      delete model.ocidList
    }
    const file = model.file;
    const ocidList = model.ocidList;
    delete model.file;
    delete model.ocidList;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);
    let query = '';
    if (ocidList) {
      for (const item of ocidList) {
        query+=`ocidList=${item}&`
      }
    }
    return this.http.post<OperationResult>(`${this.base}Account/updateForm?${query}`, params).pipe(catchError(this.handleError));
  }

}
