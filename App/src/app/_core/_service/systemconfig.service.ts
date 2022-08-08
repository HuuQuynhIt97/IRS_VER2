import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { CURDService } from './base/CURD.service';
import { SystemConfig } from '../_model/systemconfig';
@Injectable({
  providedIn: 'root'
})
export class SystemConfigService extends CURDService<SystemConfig> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"SystemConfig", utilitiesService);
  }
  getCheckedData(inOutGuid) {
    return this.http.get<string[]>(`${this.base}SystemConfig/GetCheckedData?inOutGuid=${inOutGuid}`, {});
  }

  loadDataSystem() {
    return this.http.get(`${this.base}SystemConfig/LoadDataSystem`, {});
  }
}
