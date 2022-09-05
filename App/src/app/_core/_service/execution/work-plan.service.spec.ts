/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { WorkPlanService } from './work-plan.service';

describe('Service: WorkPlan', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WorkPlanService]
    });
  });

  it('should ...', inject([WorkPlanService], (service: WorkPlanService) => {
    expect(service).toBeTruthy();
  }));
});
