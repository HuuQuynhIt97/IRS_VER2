/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ChemicalColorService } from './chemical-color.service';

describe('Service: ChemicalColor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ChemicalColorService]
    });
  });

  it('should ...', inject([ChemicalColorService], (service: ChemicalColorService) => {
    expect(service).toBeTruthy();
  }));
});
