import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PigGenetic } from '../../_model/pigs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';
@Injectable({
  providedIn: 'root'
})
export class PigGeneticService extends CURDService<PigGenetic> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"PigGenetic", utilitiesService);
  }

}
