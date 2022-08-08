import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, InjectionToken, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { MessageConstants } from '../../_constants';
import { OperationResult } from '../../_model/operation.result';
import { UtilitiesService } from '../utilities.service';

import { BaseService } from './base.service';
//#region
// Copyright Henry Pham
//#endregion
export interface ICURDService<T> {
  getAll(): Observable<T[]>;
  getById(id): Observable<T>;
  //#endregion

  //#region Action
  insertWithFormData(model: T): Observable<OperationResult>;
  updateWithFormData(model: T): Observable<OperationResult>;

  add(model: T): Observable<OperationResult>;
  update(model: T): Observable<OperationResult>;
  delete(id: any): Observable<OperationResult>;
  deleterange(ids: object[]): Observable<OperationResult>;
  changeValue(message: MessageConstants);
  getAudit(id): Observable<any>;

}
@Injectable()
export class CURDService<T> extends BaseService implements ICURDService<T> {
  audits = ['updateDate','createDate','deleteDate', 'lastLoginDate']

  protected base = environment.apiUrl;

  //#region Field
  protected _sharedHeaders = new HttpHeaders();
  //#endregion

  //#region Ctor
  constructor(
    protected http: HttpClient,
    @Inject(String) protected entity: string,
    protected utilitiesService: UtilitiesService
  ) {
    super();
    this._sharedHeaders = this._sharedHeaders.set(
      'Content-Type',
      'application/json'
    );
  }
  //#endregion

  //#region LoadData
  getAll(): Observable<T[]> {
    return this.http.get<T[]>(`${this.base}${this.entity}/getall`, {}).pipe(
      catchError(this.handleError)
    );
  }
  getById(id): Observable<T> {
    return this.http
      .get<T>(`${this.base}${this.entity}/getById?id=${id}`, {})
      .pipe(catchError(this.handleError));
  }
  //#endregion

  //#region Action
  insertWithFormData(model: T): Observable<OperationResult> {
    const params = this.utilitiesService.ToFormData(model);
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/insert`, params)
      .pipe(catchError(this.handleError));
  }
  updateWithFormData(model: T): Observable<OperationResult> {
    const params = this.utilitiesService.ToFormData(model);
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/update`, params)
      .pipe(catchError(this.handleError));
  }

  add(model: T): Observable<OperationResult> {
    for (const audit of this.audits) {
      let value2 = model[audit];
      if (value2 instanceof Date) {
        model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
      }
    }
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/add`, model);
  }
  addRange(model: T[]): Observable<OperationResult> {
    for (const audit of this.audits) {
      for (const ml of model) {
        let value2 = model[audit];
        if (value2 instanceof Date) {
          model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/addRange`, model)
      .pipe(catchError(this.handleError));
  }
  updateRange(model: T[]): Observable<OperationResult> {
    for (const audit of this.audits) {
      for (const ml of model) {
        let value2 = model[audit];
        if (value2 instanceof Date) {
          model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/updateRange`, model)
      .pipe(catchError(this.handleError));
  }
  update(model: T): Observable<OperationResult> {
    for (const audit of this.audits) {
      let value2 = model[audit];
      if (value2 instanceof Date) {
        model[audit] = `${(value2 as Date).toLocaleDateString()} ${(value2 as Date).toLocaleTimeString('en-GB')}`
      }
    }
    return this.http.post<OperationResult>(`${this.base}${this.entity}/update`, model);
  }
  updatestatus(id: T): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/updatestatus?id=${id}`, {})
      .pipe(catchError(this.handleError));
  }
  delete(id: any): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/delete?id=${id}`,{});
  }
  deleterange(ids: object[]): Observable<OperationResult> {
    let query = '';
    for (const id of ids) {
      query += `id=${id}&`;
    }
    return this.http
      .post<OperationResult>(`${this.base}${this.entity}/deleterange?${query}`,{})
      .pipe(catchError(this.handleError));
  }

  //#endregion

  getAudit(id): Observable<T> {
    return this.http
      .get<T>(`${this.base}${this.entity}/GetAudit?id=${id}`, {});
  }

  downloadODSFile(model) {
    const params = this.utilitiesService.ToFormData(model);
    return this.http
      .post(`${this.base}Files/ExcelExportToDOS`, params, { responseType: 'blob', observe: 'response' });
  }
  downloadBlob(data, fileName, mimeType) {
    var blob, url;
    blob = new Blob([data], {
      type: mimeType
    });
    url = window.URL.createObjectURL(blob);
    var a;
    a = document.createElement('a');
    a.href = url
    a.download = fileName;
    document.body.appendChild(a);
    a.style = 'display: none';
    a.click();
    a.remove();
    setTimeout(function () {
      return window.URL.revokeObjectURL(url);
    }, 1000);
  };
}
