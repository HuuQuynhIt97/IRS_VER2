/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PigKindComponent } from './pig-kind.component';

describe('PigKindComponent', () => {
  let component: PigKindComponent;
  let fixture: ComponentFixture<PigKindComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PigKindComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PigKindComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
