import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { WorkPlan } from '../../_model/execution/work-plan';
@Injectable({
  providedIn: 'root'
})
export class WorkPlanService extends CURDService<WorkPlan> {

  private bomSource = new BehaviorSubject({} as WorkPlan);
  currentWorkOrder = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"WorkPlan", utilitiesService);
  }
  changeHall(workPlan: WorkPlan) {
    this.bomSource.next(workPlan)
  }

  getAllWorkPlan() {
    return this.http.get(this.base + 'workplan/GetAllWorkPlan', {});
  }

  getAllWorkPlanNew() {
    return this.http.get(this.base + 'workplan/GetAllWorkPlanNew', {});
  }

  getAllWorkPlanWithDate(time) {
    return this.http.get(this.base + `workplan/GetAllWorkPlanWithDate/${time}`, {});
  }

  export(model) {
    return this.http.post(this.base + `workplan/WorkPlanFailedAddExport`, model , { responseType: 'blob' });
  }
  importWorkPlan(file ,time) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('Time', time);
    return this.http.post(this.base + 'WorkPlan/Import', formData);
  }

  importWorkPlanNew(file ,time) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    formData.append('Time', time);
    return this.http.post(this.base + 'WorkPlan/ImportWorkPlanNew', formData);
  }

  updatePoGlue(workPlanID) {
    return this.http.post(`${this.base}workplan/UpdatePoGlue/${workPlanID}`, {});
  }

  updatePart(workPlanID, partID) {
    return this.http.post(`${this.base}workplan/UpdatePart/${workPlanID}/${partID}`, {});
  }

  getPONumberScheduleID(scheduleID, treatment) {
    return this.http.get(this.base + `WorkPlan/GetPONumberByScheduleID/${scheduleID}/${treatment}`, {});
  }

  getPONumberScheduleIDAndPart(scheduleID, treatment, partId) {
    return this.http.get(this.base + `WorkPlan/GetPONumberByScheduleIDAndPart/${scheduleID}/${treatment}/${partId}`, {});
  }

  getPONumberScheduleIDAndPart2(scheduleID, treatment, partId) {
    return this.http.get(this.base + `WorkPlan/GetPONumberByScheduleIDAndPart2/${scheduleID}/${treatment}/${partId}`, {});
  }

  getPrintQRcodeByWorklan(id) {
    return this.http.get(this.base + `WorkPlan/GetPrintQRcodeByWorklan/${id}`, {});
  }

  getPrintQRcodeByScheduleID(id) {
    return this.http.get(this.base + `WorkPlan/GetPrintQRcodeBySchedule/${id}`, {});
  }

}
