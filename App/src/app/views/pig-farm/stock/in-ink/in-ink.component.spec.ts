/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InInkComponent } from './in-ink.component';

describe('InInkComponent', () => {
  let component: InInkComponent;
  let fixture: ComponentFixture<InInkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InInkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InInkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
