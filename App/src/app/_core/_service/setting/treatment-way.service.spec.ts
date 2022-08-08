/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TreatmentWayService } from './treatment-way.service';

describe('Service: TreatmentWay', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TreatmentWayService]
    });
  });

  it('should ...', inject([TreatmentWayService], (service: TreatmentWayService) => {
    expect(service).toBeTruthy();
  }));
});
