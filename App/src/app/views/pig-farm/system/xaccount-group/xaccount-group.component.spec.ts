/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { XaccountGroupComponent } from './xaccount-group.component';

describe('XaccountGroupComponent', () => {
  let component: XaccountGroupComponent;
  let fixture: ComponentFixture<XaccountGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ XaccountGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(XaccountGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
