import { Injectable } from '@angular/core';

import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Employee } from '../_model/employee';
import { UtilitiesService } from './utilities.service';
import { OperationResult } from '../_model/operation.result';
import { Observable, throwError } from 'rxjs';
import { catchError, first } from 'rxjs/operators';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class EmployeeService extends CURDService<Employee> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Employee", utilitiesService);
  }
  importExcel(file) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('CreatedBy', JSON.parse(localStorage.getItem('user')).id);
    return this.http.post(this.base + 'Employee/ImportExcel', formData);
  }
  importExcel3(file) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('CreatedBy', JSON.parse(localStorage.getItem('user')).id);
    return this.http.post(this.base + 'Employee/ImportExcel3', formData, {
      reportProgress: true,
      observe: 'events'
    }).pipe(
      catchError(this.errorMgmt)
    );
  }
  toggleSEAInform(id): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.base}Employee/ToggleSEAInform?id=${id}`, {});
  }
  checkin(code): Observable<OperationResult> {
    return this.http.get<OperationResult>(this.base + 'Employee/Checkin?code=' + code);
  }
  checkin2(code, testKindId): Observable<OperationResult> {
    return this.http.get<OperationResult>(`${this.base}Employee/Checkin2?code=${code}&testKindId=${testKindId}`);
  }
  ping(): Observable<any> {
    return this.http.get<any>(`${this.base.replace("/api/","")}`, { observe: 'response' }).pipe(first());
  }
  updateIsPrint(model): Observable<any> {
    return this.http.post<any>(this.base + 'Employee/UpdateIsPrint', model);
  }
  getPrintOff(): Observable<any> {
    return this.http.get<any>(this.base + 'Employee/getPrintOff', {});
  }
  countWorkerScanQRCodeByToday(): Observable<any> {
    return this.http.get<any>(`${this.base}Employee/CountWorkerScanQRCodeByToday`);
  }
  checkCode(code): Observable<boolean> {
    return this.http.get<boolean>(`${this.base}Employee/CheckCode?code=${code}`);
  }
  exportExcel() {
    return this.http.get(`${this.base}Employee/ExportEmployeeExcel`, { responseType: 'blob', observe: 'response' });
  }
  errorMgmt(error: HttpErrorResponse) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      // Get client-side error
      errorMessage = error.error.message;
    } else {
      // Get server-side error
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.log(errorMessage);
    return throwError(errorMessage);
  }
  getEmployeesByAccountID(accountID): Observable<any> {
    return this.http.get<any>(`${this.base}Employee/GetEmployeesByAccountID?accountID=${accountID}`, {});
  }
  getEmployees(): Observable<any> {
    return this.http.get<any>(this.base + 'Employee/getEmployees', {});
  }
  checkExist(no): Observable<OperationResult> {
    return this.http.get<OperationResult>(`${this.base}Employee/CheckExist?no=${no}`, {});
  }
}
