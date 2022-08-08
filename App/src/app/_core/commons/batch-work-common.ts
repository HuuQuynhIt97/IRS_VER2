import { Directive, ViewChild } from "@angular/core";

export interface IBatchWorkCommon {
  pigGuid: string;
  penGuid: string;
  estDate: any;
  pens: any;
  pigs: any;
  penTemplate: any;
  pigTemplate: any;
  estDateTemplate: any;
  onSelectedPenValue(args): any;
  onSelectedPigValue(args): any;
  onSelectedEstDateValue(args): any;
}
@Directive()
export abstract class BatchWorkCommon implements IBatchWorkCommon {
  @ViewChild('estDateTemplate', {static:true}) public estDateTemplate: any;
  @ViewChild('penTemplate', {static:true}) public penTemplate: any;
  @ViewChild('pigTemplate', {static:true}) public pigTemplate: any;
  pigGuid: string;
  penGuid: string;
  estDate: any;
  pens: any;
  pigs: any;
  toolbarOptions: any;
  constructor() {
    this.toolbarOptions =['Add',
    {template: this.penTemplate},
    {template: this.pigTemplate},
    {template: this.estDateTemplate},
    'Search'
  ];
  }

  onSelectedPenValue(args) {
    this.penGuid = args;
  };
  onSelectedPigValue(args) {
    this.pigGuid = args;

  };
  onSelectedEstDateValue(args) {
    this.estDate = args;

  };
}
