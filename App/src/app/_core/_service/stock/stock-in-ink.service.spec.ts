/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StockInInkService } from './stock-in-ink.service';

describe('Service: StockInInk', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StockInInkService]
    });
  });

  it('should ...', inject([StockInInkService], (service: StockInInkService) => {
    expect(service).toBeTruthy();
  }));
});
