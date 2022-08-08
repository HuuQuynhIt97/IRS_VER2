import { BookingDetail } from './../../_model/booking/bookingDetail';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { WorkOrder } from '../../_model/booking/workOrder';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BookingService extends CURDService<BookingDetail>{

  private bomSource = new BehaviorSubject({} as BookingDetail);
  currentBooking = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Booking", utilitiesService);
  }
  getImage(imageUrl: string): Observable<Blob> {
    return this.http.get(imageUrl, { responseType: 'blob' });
  }
  booking(booking: BookingDetail) {
    this.bomSource.next(booking)
  }
  checkImage(path) {
    return this.http.get<any>(`${this.base}Booking/DoesImageExistRemotely?uriToImage=${path}`, {});
  }
  getPreviewScheduler(siteID) {
    return this.http.get(`${this.base}Booking/GetPreviewScheduler?siteGuid=${siteID}`, {});
  } 

  search(filter) {
    return this.http.post(`${this.base}Booking/Search`, filter);
  } 

  searchBookingCheck(filter) {
    return this.http.post(`${this.base}Booking/SearchBookingCheck`, filter);
  } 

  getSearchBookingCalendar(filter) {
    return this.http.post(`${this.base}Booking/GetSearchBookingCalendar`, filter);
  } 
  getSearchByList(filter) {
    return this.http.post(`${this.base}Booking/getSearchByList`, filter);
  } 
  searchRoomDetail(filter) {
    return this.http.post(`${this.base}Booking/SearchDetailRoom`, filter);
  } 

  getBookingCheck() {
    return this.http.get(`${this.base}Booking/GetBookingCheck`, {});
  } 
 
}
