import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TransactionRoutingModule } from './transaction-routing.module';
import { TransactionComponent } from './transaction.component';
import { NameRecipeWorkerComponent } from './name-recipe-worker/name-recipe-worker.component';
import { NameChildComponent } from './name-recipe-worker/name-child/name-child.component';
import { RecipeChildComponent } from './name-recipe-worker/recipe-child/recipe-child.component';
import { FormsModule } from '@angular/forms';
import { CoreDirectivesModule } from 'src/app/_core/_directive/core.directives.module';
import { SharedModule } from 'src/app/_core/commons/shared.module';
import { Common2Module } from 'src/app/_core/commons/common2.module';


@NgModule({
  declarations: [
    TransactionComponent,
    NameRecipeWorkerComponent,
    NameChildComponent,
    RecipeChildComponent
  ],
  imports: [
    CommonModule,
    TransactionRoutingModule,
    FormsModule,
    CoreDirectivesModule,
    SharedModule.forRoot(),
    Common2Module.forRoot()
  ]
})
export class TransactionModule { }
