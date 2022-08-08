import { TestBed } from '@angular/core/testing';

import { GlueService } from './glue.service';

describe('GlueService', () => {
  let service: GlueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
