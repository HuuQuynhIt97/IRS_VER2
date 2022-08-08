import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { CURDService } from './base/CURD.service';
import { StoredProcedure } from '../_model/stored-procedure';
@Injectable({
  providedIn: 'root'
})
export class StoredProcedureService extends CURDService<StoredProcedure> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"StoredProcedure", utilitiesService);
  }
}
