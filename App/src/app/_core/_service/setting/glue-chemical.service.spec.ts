import { TestBed } from '@angular/core/testing';

import { GlueChemicalService } from './glue-chemical.service';

describe('GlueChemicalService', () => {
  let service: GlueChemicalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GlueChemicalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
