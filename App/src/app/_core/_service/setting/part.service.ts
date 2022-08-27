import { Part } from './../../_model/setting/part';
import { Color } from './../../_model/setting/color';
import { Shoe } from './../../_model/setting/shoe';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class PartService  extends CURDService<Part>{

  private colorSource = new BehaviorSubject({} as Part);
  currentPart = this.colorSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Part", utilitiesService);
  }
  changeColor(part: Part) {
    this.colorSource.next(part)
  }
  getAllPart(lang) {
    return this.http.get<any>(`${this.base}Part/GetAllPart?lang=${lang}`, {});
  }
}
