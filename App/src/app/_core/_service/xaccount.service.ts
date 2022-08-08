import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
import { Profile, XAccount } from '../_model/xaccount';
@Injectable({
  providedIn: 'root'
})
export class XAccountService extends CURDService<XAccount> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"XAccount", utilitiesService);
  }

  getXAccountsBySite(siteID) {
    return this.http.get<any>(`${this.base}XAccount/getXAccountsBySite?siteID=${siteID}`, {});
  }
  showPassword(key): Observable<any> {
    const params = new FormData();
    params.append("key", key);
    return this.http.post<any>(`${this.base}XAccount/ShowPassword`, params).pipe(catchError(this.handleError));
  }
  changePassword(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(`${this.base}XAccount/changePassword`, model).pipe(catchError(this.handleError));
  }
  insertForm(model: XAccount): Observable<OperationResult> {
    for (const key in model) {
      if (Object.prototype.hasOwnProperty.call(model, key)) {
        let item = model[key];
        if (item instanceof Date) {
          model[key] = `${(item as Date).toLocaleDateString()} ${(item as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
    const file = model.file;
    delete model.file;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);
    return this.http.post<OperationResult>(`${this.base}XAccount/AddForm`, params).pipe(catchError(this.handleError));
  }
  updateForm(model: XAccount): Observable<OperationResult> {
    for (const key in model) {
      if (Object.prototype.hasOwnProperty.call(model, key)) {
        let item = model[key];
        if (item instanceof Date) {
          model[key] = `${(item as Date).toLocaleDateString()} ${(item as Date).toLocaleTimeString('en-GB')}`
        }
      }
    }
   
    const file = model.file;
    delete model.file;
    const params = this.utilitiesService.ToFormData(model);
    params.append("file", file);

    return this.http.post<OperationResult>(`${this.base}XAccount/updateForm`, params).pipe(catchError(this.handleError));
  }
  getPermissionsDropdown(accountGuid, lang) {
    return this.http.get<any>(`${this.base}XAccount/getPermissionsDropdown?lang=${lang}&accountGuid=${accountGuid}`, {});
  }
  getPermissions(accountGuid, lang) {
    return this.http.get<any>(`${this.base}XAccount/getPermissions?lang=${lang}&accountGuid=${accountGuid}`, {});
  }
  storePermission(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}XAccount/StorePermission`, model)
      .pipe(catchError(this.handleError));
  }
  getProfile(key) {
    return this.http.get<any>(`${this.base}XAccount/GetProfile?key=${key}`, {});
  }
  storeProfile(model: Profile): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}XAccount/StoreProfile`, model)
      .pipe(catchError(this.handleError));
  }

  getRejectsByAcceptance(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/getRejectsByAcceptance?farmGuid=${farmGuid}`, {});
  }
  getRejectsByRequisition(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetRejectsByRequisition?farmGuid=${farmGuid}`, {});
  }
  getRejectsByRepair(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetRejectsByRepair?farmGuid=${farmGuid}`, {});
  }
  getRejectsBySalesOrder(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetRejectsBySalesOrder?farmGuid=${farmGuid}`, {});
  }
  getRejectsByPigDisease(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetRejectsByPigDisease?farmGuid=${farmGuid}`, {});
  }
  getApproveByPigDisease(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetApproveByPigDisease?farmGuid=${farmGuid}`, {});
  }
  getRecordByPigDisease(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetRecordByPigDisease?farmGuid=${farmGuid}`, {});
  }
  getXAccountsForDropdown(farmGuid) {
    return this.http.get<any>(`${this.base}XAccount/GetXAccountsForDropdown?farmGuid=${farmGuid}`, {});
  }


}
