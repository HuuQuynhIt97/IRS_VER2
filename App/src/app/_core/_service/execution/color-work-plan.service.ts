// import { environment } from './../../../../environments/environment.prod';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { environment } from 'src/environments/environment';
// import { environment } from 'src/environments/environment';
import { UtilitiesService } from '../utilities.service';

import { Observable } from 'rxjs';
import { OperationResult } from '../../_model/operation.result';

@Injectable({
  providedIn: 'root'
})
export class ColorWorkPlanService  {
  public base = environment.apiUrl

  constructor(private http: HttpClient, utilitiesService: UtilitiesService, )
  {

  }






  getAllShoes() {
    return this.http.get(this.base + 'ColorWorkPlan/LoadShoes', {});
  }

  addColorWorkPlan2(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.base + 'ColorWorkPlan/AddColorWorkPlan', model);
  }

  addColorWorkPlan(model): Observable<OperationResult> {
    return this.http.post<OperationResult>(this.base + 'ColorWorkPlan/AddColorWorkPlan', model);
  }

  updateColorWorkPlan(model): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.base + 'ColorWorkPlan/UpdateColorWorkPlan', model);
  }

  deleteColorWorkPlan(id: any): Observable<OperationResult> {
    return this.http.delete<OperationResult>(`${this.base}ColorWorkPlan/DeleteColorWorkPlan/${id}`,{});
  }

  createColorTodo(currentTime: any) {
    return this.http.get(this.base + `ColorWorkPlan/StoreProcedureCreateColorTodo?currentDate=` + currentTime, {});
  }

  loadColorToDo() {
    return this.http.get(this.base + `ColorWorkPlan/LoadColorToDo`, {});
  }

  updateIsFinishedColorToDo(model): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.base + `ColorWorkPlan/UpdateIsFinishedColorToDo`, model);
  }

  updateColorToDoAmount(model): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.base + `ColorWorkPlan/UpdateColorToDoAmount`, model);
  }

}
