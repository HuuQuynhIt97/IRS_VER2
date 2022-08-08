/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { InkService } from './ink.service';

describe('Service: Ink', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InkService]
    });
  });

  it('should ...', inject([InkService], (service: InkService) => {
    expect(service).toBeTruthy();
  }));
});
