import { Shoe } from './../../_model/setting/shoe';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CURDService } from '../base/CURD.service';
import { UtilitiesService } from '../utilities.service';


@Injectable({
  providedIn: 'root'
})
export class ShoeService extends CURDService<Shoe> {

  private shoeSource = new BehaviorSubject({} as Shoe);
  currentShoe = this.shoeSource.asObservable();
  constructor( http: HttpClient,utilitiesService: UtilitiesService)
  {
    super(http,"Shoe", utilitiesService);
  }
  changeShoe(shoe: Shoe) {
    this.shoeSource.next(shoe)
  }

  
}
