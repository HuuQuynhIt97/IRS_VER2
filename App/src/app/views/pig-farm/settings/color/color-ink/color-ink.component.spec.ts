/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ColorInkComponent } from './color-ink.component';

describe('ColorInkComponent', () => {
  let component: ColorInkComponent;
  let fixture: ComponentFixture<ColorInkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorInkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorInkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
