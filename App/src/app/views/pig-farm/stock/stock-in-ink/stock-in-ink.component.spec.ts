/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { StockInInkComponent } from './stock-in-ink.component';

describe('StockInInkComponent', () => {
  let component: StockInInkComponent;
  let fixture: ComponentFixture<StockInInkComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StockInInkComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StockInInkComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
