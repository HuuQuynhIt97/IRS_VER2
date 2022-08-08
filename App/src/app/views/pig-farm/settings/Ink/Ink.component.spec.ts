/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { InkComponent } from './Ink.component';

describe('InkComponent', () => {
  let component: InkComponent;
  let fixture: ComponentFixture<InkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
