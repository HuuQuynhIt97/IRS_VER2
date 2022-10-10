import { TestBed } from '@angular/core/testing';

import { ColorWorkPlanService } from './color-work-plan.service';

describe('ColorWorkPlanService', () => {
  let service: ColorWorkPlanService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ColorWorkPlanService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
