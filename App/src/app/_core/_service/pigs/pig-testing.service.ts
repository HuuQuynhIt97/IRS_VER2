import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PigTesting } from '../../_model/pigs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PigTestingService extends CURDService<PigTesting> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"PigTesting", utilitiesService);
  }

}
