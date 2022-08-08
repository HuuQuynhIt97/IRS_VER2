import { Room, RoomScreen } from './../../_model/club-management/room';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../../_model/operation.result';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class RoomService extends CURDService<Room> {

  private bomSource = new BehaviorSubject({} as RoomScreen);
  currentSite = this.bomSource.asObservable();
  constructor(http: HttpClient, utilitiesService: UtilitiesService) {
    super(http, "Room", utilitiesService);
  }
  changeRoom(room: Room) {
    this.bomSource.next(room)
  }
  search(filter) {
    return this.http.post(`${this.base}Room/Search`, filter);
  } 
  insertForm(model: Room): Observable<OperationResult> {
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
    if (model.roomGallery?.length > 0) {
      for (var i = 0; i < model.roomGallery?.length; i++) {
        params.append('RoomGallery', model.roomGallery[i]);
      }
    }
    return this.http.post<OperationResult>(`${this.base}Room/AddForm`, params).pipe(catchError(this.handleError));
  }

  updateForm(model: Room): Observable<OperationResult> {
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
    const formData = this.utilitiesService.ToFormData(model);
    formData.append("file", file);
    if (model.roomGallery?.length > 0) {
      for (var i = 0; i < model.roomGallery?.length; i++) {
        formData.append('RoomGallery', model.roomGallery[i]);
      }
    }
    return this.http.post<OperationResult>(`${this.base}Room/updateForm`, formData).pipe(catchError(this.handleError));
  }

}
