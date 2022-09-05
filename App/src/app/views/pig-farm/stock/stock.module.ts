import { StockInChemicalComponent } from './stock-in-chemical/stock-in-chemical.component';
import { StockInInkComponent } from './stock-in-ink/stock-in-ink.component';
import { AutofocusQrcodeDirective } from './../../../_core/_directive/selectQrcode.directive';
import { InChemicalComponent } from './in-chemical/in-chemical.component';
import { InInkComponent } from './in-ink/in-ink.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StockRoutingModule } from './stock-routing.module';
import { StockComponent } from './stock.component';
import { FormsModule } from '@angular/forms';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';


@NgModule({
  declarations: [
    StockComponent,
    InInkComponent,
    InChemicalComponent,
    StockInInkComponent,
    StockInChemicalComponent,
    AutofocusQrcodeDirective
  ],
  imports: [
    CommonModule,
    StockRoutingModule,
    FormsModule,
    CoreDirectivesModule,
    SharedModule.forRoot(),
    Common2Module.forRoot()
  ]
})
export class StockModule { }
