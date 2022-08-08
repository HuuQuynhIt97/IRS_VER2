
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Room, RoomScreen } from '../../_model/farms';
import { OperationResult } from '../../_model/operation.result';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';


@Injectable({
  providedIn: 'root'
})
export class RoomService extends CURDService<Room> {
  private bomSource = new BehaviorSubject({} as RoomScreen);
  currentRoom = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Room", utilitiesService);
  }

  getRoomByid(id) {
    return this.http.get<any>(`${this.base}Room/getRoomByid?ID=${id}`, {});
  }
  getRoomBySiteAndHall(siteID,hallID) {
    return this.http.get<any>(`${this.base}Room/GetRoomBySiteAndHall?siteID=${siteID}&hallID=${hallID}`, {});
  }
  changeRoom(farm: RoomScreen) {
    this.bomSource.next(farm)
  }
  getRoomByRecord(recordGuid, type): Observable<any> {
    return this.http.get<any>(`${this.base}Room/GetRoomByRecord?recordGuid=${recordGuid}&type=${type}`, {});
  }
  addRecord2Room(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Room/AddRecord2Room`, model)
      .pipe(catchError(this.handleError));
  }

  removeRecord2Room(model): Observable<OperationResult> {
    return this.http
      .post<OperationResult>(`${this.base}Room/RemoveRecord2Room`, model)
      .pipe(catchError(this.handleError));
  }
}
