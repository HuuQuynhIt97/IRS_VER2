/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StockInChemicalService } from './stock-in-chemical.service';

describe('Service: StockInChemical', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StockInChemicalService]
    });
  });

  it('should ...', inject([StockInChemicalService], (service: StockInChemicalService) => {
    expect(service).toBeTruthy();
  }));
});
