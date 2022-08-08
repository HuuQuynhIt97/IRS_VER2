import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Glue } from '../../_model/setting/glue';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';

@Injectable({
  providedIn: 'root'
})
export class GlueService extends CURDService<Glue> {

  private glueSource = new BehaviorSubject({} as Glue);
  currentGlue = this.glueSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Glues", utilitiesService);
  }
  changeGlue(glue: Glue) {
    this.glueSource.next(glue)
  }

 
}
