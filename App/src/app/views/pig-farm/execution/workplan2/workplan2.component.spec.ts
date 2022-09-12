/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Workplan2Component } from './workplan2.component';

describe('Workplan2Component', () => {
  let component: Workplan2Component;
  let fixture: ComponentFixture<Workplan2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Workplan2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Workplan2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
