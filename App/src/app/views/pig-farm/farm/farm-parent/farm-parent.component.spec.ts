/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { FarmParentComponent } from './farm-parent.component';

describe('FarmParentComponent', () => {
  let component: FarmParentComponent;
  let fixture: ComponentFixture<FarmParentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FarmParentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FarmParentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
