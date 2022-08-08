import { TestBed } from '@angular/core/testing';

import { ShoeGlueService } from './shoe-glue.service';

describe('ShoeGlueService', () => {
  let service: ShoeGlueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ShoeGlueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
