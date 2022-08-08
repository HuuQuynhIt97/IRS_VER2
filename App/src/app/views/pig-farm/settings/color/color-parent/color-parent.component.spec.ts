/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ColorParentComponent } from './color-parent.component';

describe('ColorParentComponent', () => {
  let component: ColorParentComponent;
  let fixture: ComponentFixture<ColorParentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ColorParentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ColorParentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
