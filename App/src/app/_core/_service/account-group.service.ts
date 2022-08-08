import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { AccountGroup } from '../_model/account-group';
import { UtilitiesService } from './utilities.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OperationResult } from '../_model/operation.result';
import { CURDService } from './base/CURD.service';
@Injectable({
  providedIn: 'root'
})
export class AccountGroupService extends CURDService<AccountGroup> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"AccountGroup", utilitiesService);
  }

}
