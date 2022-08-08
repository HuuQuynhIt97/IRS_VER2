import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PigCode } from '../../_model/pigs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PigCodeService extends CURDService<PigCode> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"PigCode", utilitiesService);
  }

}
