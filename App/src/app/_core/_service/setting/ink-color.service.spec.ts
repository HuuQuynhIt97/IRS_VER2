/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { InkColorService } from './ink-color.service';

describe('Service: InkColor', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InkColorService]
    });
  });

  it('should ...', inject([InkColorService], (service: InkColorService) => {
    expect(service).toBeTruthy();
  }));
});
