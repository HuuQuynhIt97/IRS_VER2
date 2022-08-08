import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { UtilitiesService } from './utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  baseUrl = environment.apiUrl;
  ModalNameSource = new BehaviorSubject<number>(0);
  currentModalName = this.ModalNameSource.asObservable();
  lang: any;
  userid: any;
  constructor(
    private http: HttpClient, private utilitiesService: UtilitiesService
  ) {
    this.lang = localStorage.getItem('lang');
   }
// #region Permisison
  getAllPermission() {
    return this.http.get(this.baseUrl + 'Permission/GetAll', {});
  }
  create(model) {
    return this.http.post(this.baseUrl + 'Permission/Create', model);
  }
  update(model) {
    return this.http.post(this.baseUrl + 'Permission/Update', model);
  }
  delete(id: number) {
    return this.http.post(this.baseUrl + 'Permission/Delete/' + id, {});
  }
// #endregion

  // #region Module
  getAllModule() {
    return this.http.get(this.baseUrl + 'Permission/GetAllModule', {});
  }
  createModule(model) {
    return this.http.post(this.baseUrl + 'Permission/CreateModule', model);
  }
  updateModule(model) {
    return this.http.post(this.baseUrl + 'Permission/UpdateModule', model);
  }
  deleteModule(id: number) {
    return this.http.post(this.baseUrl + 'Permission/DeleteModule/' + id, {});
  }
// #endregion

  // #region Function
  getAllFunction() {
    return this.http.get(this.baseUrl + 'Permission/GetAllFunction', {});
  }
  getFunctionsAsTreeView() {
    return this.http.get(this.baseUrl + 'Permission/GetFunctionsAsTreeView', {});
  }
  createFunction(model) {
    return this.http.post(this.baseUrl + 'Permission/CreateFunction', model);
  }
  updateFunction(model) {
    return this.http.post(this.baseUrl + 'Permission/UpdateFunction', model);
  }
  deleteFunction(id: number) {
    return this.http.post(this.baseUrl + 'Permission/DeleteFunction/' + id, {});
  }
  getActionInFunctionByRoleID(id: number) {
    return this.http.get( `${this.baseUrl}Permission/GetActionInFunctionByRoleID/${id}`);
  }
// #endregion


  // #region Action
  getAllAction() {
    return this.http.get(this.baseUrl + 'Permission/GetAllAction', {});
  }
  createAction(model) {
    return this.http.post(this.baseUrl + 'Permission/CreateAction', model);
  }
  updateAction(model) {
    return this.http.post(this.baseUrl + 'Permission/UpdateAction', model);
  }
  deleteAction(id: number) {
    return this.http.post(this.baseUrl + 'Permission/DeleteAction/' + id, {});
  }
  // #endregion

  getMenuByUserPermission(userid, lang) {
    return this.http.get<[]>(`${this.baseUrl}Permission/GetMenuByUserPermission/${userid}/${lang}`, {}).pipe(map(response => {
      const menus = response;
      return menus;
    }));
  }

  putPermissionByRoleId(roleID, request) {
    return this.http.post(this.baseUrl + 'Permission/putPermissionByRoleId/' + roleID, request);
  }
  postActionToFunction(functionID, request) {
    return this.http.post(this.baseUrl + 'Permission/PostActionToFunction/' + functionID, request);
  }
  deleteActionToFunction(functionID, request) {
    return this.http.post(`${this.baseUrl}Permission/deleteActionToFunction/${functionID}?actionIds=${request.actionIds}`,{});
  }
  getScreenAction(functionID) {
    return this.http.get<[]>(this.baseUrl + 'Permission/GetScreenAction/' + functionID, {});
  }
  getScreenFunctionAndAction(roleIDs: any) {
    let lang = this.lang;
    return this.http.post(`${this.baseUrl}Permission/GetScreenFunctionAndAction`, {
      roleIDs,
      lang
    }).pipe(map((data: any[]) => {
      const data2 = data;
      for (const item of data2) {
        const checkedNodes = [];
        for (const subItem of item.fields.dataSource) {
          for (const child of subItem.childrens) {
            child.isChecked = child.status;
            if (child.status) {
              checkedNodes.push(child.id);
            }
          }
          const flag = subItem.childrens.every(v => v.status === true);
          if (flag) {
            checkedNodes.push(subItem.id);
            subItem.isChecked = flag;
          }
        }
      }
      return data2;
    }));
  }

   loadLanguages() {
    return this.http.get(this.baseUrl + 'SystemLanguage/LoadLanguages', {});
  }
}
