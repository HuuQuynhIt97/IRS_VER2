/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { WorkPlan2Service } from './work-plan2.service';

describe('Service: WorkPlan2', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WorkPlan2Service]
    });
  });

  it('should ...', inject([WorkPlan2Service], (service: WorkPlan2Service) => {
    expect(service).toBeTruthy();
  }));
});
