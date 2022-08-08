/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { InChemicalService } from './inChemical.service';

describe('Service: InChemical', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InChemicalService]
    });
  });

  it('should ...', inject([InChemicalService], (service: InChemicalService) => {
    expect(service).toBeTruthy();
  }));
});
