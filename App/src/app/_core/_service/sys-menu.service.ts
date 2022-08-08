import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SysMenu } from '../_model/sys-menu';
import { UtilitiesService } from './utilities.service';
import { CURDService } from './base/CURD.service';
import { of } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SysMenuService extends CURDService<SysMenu> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"SysMenu", utilitiesService);
  }
  getParents(lang) {
    return this.http.get<any>(`${this.base}SysMenu/getParents?lang=${lang}`, {});
  }
  getToolbarParents(lang) {
    return this.http.get<any>(`${this.base}SysMenu/getToolbarParents?lang=${lang}`, {});
  }
  getMenus(lang) {
    return this.http.get<any>(`${this.base}SysMenu/getMenus?lang=${lang}`, {});
  }
  getMenusByFarm(lang,farmGuid) {
    return this.http.get<any>(`${this.base}SysMenu/getMenusByFarm?lang=${lang}&farmGuid=${farmGuid}`, {});
  }
  getItemByKind(lang,kind) {
    return this.http.get<any>(`${this.base}SysMenu/GetItemByKind?lang=${lang}&kind=${kind}`, {});
  }


}
