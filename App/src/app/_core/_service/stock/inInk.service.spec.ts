/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { InInkService } from './inInk.service';

describe('Service: InInk', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InInkService]
    });
  });

  it('should ...', inject([InInkService], (service: InInkService) => {
    expect(service).toBeTruthy();
  }));
});
