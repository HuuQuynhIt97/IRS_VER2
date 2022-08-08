
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CullingTank, CullingTankScreen } from '../../_model/farms';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';


@Injectable({
  providedIn: 'root'
})
export class CullingTankService extends CURDService<CullingTank> {
  private bomSource = new BehaviorSubject({} as CullingTankScreen);
  currentCullingTank = this.bomSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"CullingTank", utilitiesService);
  }
  changeCullingTank(farm: CullingTankScreen) {
    this.bomSource.next(farm)
  }
  // toggleIsDefault(id): Observable<OperationResult> {
  //   return this.http.post<OperationResult>(`${this.base}BOM/ToggleIsDefault?id=${id}`, {}).pipe(
  //     catchError(this.handleError)
  //   );
  // }
}
