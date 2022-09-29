import { Color } from './../../_model/setting/color';
import { Shoe } from './../../_model/setting/shoe';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ColorService extends CURDService<Color> {

  private colorSource = new BehaviorSubject({} as Color);
  currentColor = this.colorSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Color", utilitiesService);
  }
  changeColor(color: Color) {
    this.colorSource.next(color)
  }
  getToolTip(tooltip) {
    return this.http.post(`${this.base}Color/GetToolTip/`, tooltip);
  }

  getToolTip2(tooltip) {
    return this.http
      .post(`${this.base}Color/GetToolTip/`, tooltip)
      .pipe(
        map((res: any) => {
          if (res.length > 0) {
            return res.join('<br>');
          } else {
            return 'N/A';
          }
        })
      );
  }
  import(file) {
    const formData = new FormData();
    formData.append('UploadedFile', file);
    return this.http.post(this.base + 'Color/Import', formData);
  }

}
