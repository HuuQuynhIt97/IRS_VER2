/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CullingTankComponent } from './culling-tank.component';

describe('CullingTankComponent', () => {
  let component: CullingTankComponent;
  let fixture: ComponentFixture<CullingTankComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CullingTankComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CullingTankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
