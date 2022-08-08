/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { Process2Service } from './process2.service';

describe('Service: Process2', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Process2Service]
    });
  });

  it('should ...', inject([Process2Service], (service: Process2Service) => {
    expect(service).toBeTruthy();
  }));
});
