import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SystemLanguage } from '../_model/system-language';
import { UtilitiesService } from './utilities.service';
import { CURDService } from './base/CURD.service';
import { Observable, of } from 'rxjs';
import { OperationResult } from '../_model/operation.result';
@Injectable({
  providedIn: 'root'
})
export class SystemLanguageService extends CURDService<SystemLanguage> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"SystemLanguage", utilitiesService);
  }
  loadLanguages(lang) {
    const isAdminLang = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_LANGUAGE';
    if (isAdminLang) {
      return of({});
    }
    return this.http.get<any>(`${this.base}SystemLanguage/loadLanguages?lang=${lang}`, {});
  }
  getLanguages(lang) {
    const isAdminLang = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_LANGUAGE';
    if (isAdminLang) {
      return of({});
    }
    return this.http.get<any>(`${this.base}SystemLanguage/getLanguages?lang=${lang}`, {});
  }

  
  getReportColumns(menuLink,lang) {
    return this.http.get<any>(`${this.base}SystemLanguage/GetReportColumns?menuLink=${menuLink}&lang=${lang}`, {});
  }
  getPages(lang) {
    return this.http.get<any>(`${this.base}SystemLanguage/GetPages?lang=${lang}`, {});
  }
  getTypes(lang) {
    return this.http.get<any>(`${this.base}SystemLanguage/GetTypes?lang=${lang}`, {});
  }
  updateBySequence(systemMenuGuid, fromSequence, dropSequence): Observable<OperationResult> {
    const query = `systemMenuGuid=${systemMenuGuid}&fromSequence=${+fromSequence + 1}&dropSequence=${+dropSequence + 1}`
    return this.http
      .get<OperationResult>(`${this.base}${this.entity}/UpdateBySequence?${query}`, {});
  }
}
