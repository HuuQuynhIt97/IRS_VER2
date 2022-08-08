import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UtilitiesService } from './utilities.service';
import { CURDService } from './base/CURD.service';
import { of } from 'rxjs';
import { CodePermission } from '../_model/code-permission';
@Injectable({
  providedIn: 'root'
})
export class CodePermissionService extends CURDService<CodePermission> {

  constructor(http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"CodePermission", utilitiesService);
  }
}
