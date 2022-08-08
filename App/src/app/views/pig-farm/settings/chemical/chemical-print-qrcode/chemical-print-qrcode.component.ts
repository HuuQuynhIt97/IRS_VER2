import { Chemical } from './../../../../../_core/_model/setting/chemical';
import { ChemicalService } from './../../../../../_core/_service/setting/chemical.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DisplayTextModel, QRCodeGenerator } from '@syncfusion/ej2-angular-barcode-generator';
import { DatePipe } from '@angular/common';
import { TextBoxComponent } from '@syncfusion/ej2-angular-inputs';
import { Location } from '@angular/common'
@Component({
  selector: 'app-chemical-print-qrcode',
  templateUrl: './chemical-print-qrcode.component.html',
  styleUrls: ['./chemical-print-qrcode.component.scss'],
  providers: [DatePipe]
})
export class ChemicalPrintQrcodeComponent implements OnInit {

  public qrcode = '';
  public batch = 'DEFAULT';
  public mfgTemp = new Date();
  public mfg = this.datePipe.transform(this.mfgTemp, 'yyyyMMdd');
  public exp = this.datePipe.transform(new Date(new Date().setMonth(new Date().getMonth() + 4)), 'yyyyMMdd');
  public ingredient: Chemical = {} as Chemical;
  @ViewChild('barcode')
  public barcode: QRCodeGenerator;
  @ViewChild('displayText')
  public displayText: TextBoxComponent;
  public displayTextMethod: DisplayTextModel = {
    visibility: false
  };
  name: any;
  constructor(
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    private location: Location,
    private chemicalService: ChemicalService,
  ) {
  }

  ngOnInit(): void {
    this.onRouteChange();
  }
  back(): void {
    this.location.back()
  }
  getByID(Id) {
    this.chemicalService.getById(Id)
      .subscribe((res: any) => {
        this.ingredient = res;
        this.mfg = this.datePipe.transform(this.mfgTemp, 'yyyyMMdd');
        this.exp = this.datePipe.transform(this.mfgTemp.setDate(this.mfgTemp.getDate() + this.ingredient.daysToExpiration), 'yyyyMMdd');
        this.qrcode = `${this.mfg}-${this.batch}-${this.ingredient.code}`;
      }, error => {
      });
  }

  onChangeProductionDate(args) {
    if (args.isInteracted) {
      const pd = args.value as Date;
      this.mfg = this.datePipe.transform(pd, 'yyyyMMdd');
      this.exp = this.datePipe.transform(pd.setDate(pd.getDate() + this.ingredient.daysToExpiration), 'yyyyMMdd');
      this.qrcode = `${this.mfg}-${this.batch}-${this.ingredient.code}`;
    }
  }

  printData() {
    const printContent = document.getElementById('qrcode');
    const WindowPrt = window.open('', '_blank', 'left=0,top=0,width=1000,height=900,toolbar=0,scrollbars=0,status=0');
    // WindowPrt.document.write(printContent.innerHTML);
    WindowPrt.document.write(`
    <html>
      <head>
      </head>
      <style>
        * {
          box-sizing: border-box;
          -moz-box-sizing: border-box;
        }

        .content {
          page-break-after: always;
          clear: both;
        }

        .content .qrcode {
          float:left;
          width: 100px;
          margin-top: 10px;
          padding: 0;
          margin-left: 0px;
        }

        .content .info {
          float:left;
          list-style: none;
          width: 200px;
        }
        .content .info ul {
          float:left;
          list-style: none;
          padding: 0px;
          margin: 0px;
          margin-top: 20px;
          font-weight: bold;
          word-wrap: break-word;
        }

        @page {
          size: 2.65 1.20 in;
          page-break-after: always;
          margin: 0;
        }
        @media print {
          html, body {
            width: 90mm; // Chi co nhan millimeter
          }
        }
      </style>
      <body onload="window.print(); window.close()">
      <div class='content'>
        <div class='qrcode'>
         ${printContent.innerHTML}
         </div>
          <div class='info'>
          <ul>
            <li class='subInfo'>Name: ${this.ingredient.name}</li>
            <li class='subInfo'>QR Code: ${this.qrcode}</li>
            <li class='subInfo'>MFG: ${this.mfg}</li>
            <li class='subInfo'>EXP: ${this.exp}</li>
          </ul>
         </div>
      </div>
      </body>
    </html>
    `);
    WindowPrt.document.close();
    // WindowPrt.focus();
    // WindowPrt.print();
    // WindowPrt.close();
  }

  onRouteChange() {
    this.route.data.subscribe(data => {
      this.getByID(this.route.snapshot.params.id);
      this.name = this.route.snapshot.params.name;
    });
  }

  onChangeBatch(args) {
    this.qrcode = `${this.mfg}-${args}-${this.route.snapshot.paramMap.get('code')}`;
  }

}
