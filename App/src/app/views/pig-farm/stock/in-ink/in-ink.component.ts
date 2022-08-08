import { TranslateService } from '@ngx-translate/core';
import { InkService } from './../../../../_core/_service/setting/ink.service';
import { InInkService } from './../../../../_core/_service/stock/inInk.service';
import { debounceTime } from 'rxjs/operators';
import { Query } from '@syncfusion/ej2-data';
import { EmitType } from '@syncfusion/ej2-base';
import { DatePipe } from '@angular/common';
import { ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DisplayTextModel } from '@syncfusion/ej2-angular-barcode-generator';
import { DropDownListComponent, FilteringEventArgs } from '@syncfusion/ej2-angular-dropdowns';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { Subject, Subscription } from 'rxjs';
import { InkScanner } from 'src/app/_core/_model/stock/inkScanner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { AuthService } from 'src/app/_core/_service/auth.service';
import { Ink } from 'src/app/_core/_model/setting/ink';
import { InInk } from 'src/app/_core/_model/stock/inInk';
import { BaseComponent } from 'src/app/_core/_component/base.component';

@Component({
  selector: 'app-in-ink',
  templateUrl: './in-ink.component.html',
  styleUrls: ['./in-ink.component.scss'],
  providers: [
    DatePipe
  ]
})
export class InInkComponent extends BaseComponent implements OnInit, OnDestroy {
  @ViewChild('ddlelement')
  public dropDownListObject: DropDownListComponent;
  @ViewChild('scanQRCode') scanQRCodeElement: ElementRef;
  public displayTextMethod: DisplayTextModel = {
    visibility: false
  };
  // public filterSettings: object;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 20 };
  @ViewChild('grid') public grid: GridComponent;
  // toolbarOptions: string[];
  @ViewChild('scanText', { static: false }) scanText: ElementRef;
  @ViewChild('ingredientinfoGrid') ingredientinfoGrid: GridComponent;
  qrcodeChange: any;
  data: [];
  dataOut: [];
  checkout = false;
  checkin = true;
  public ink: any = [];
  test: any = 'form-control w3-light-grey';
  checkCode: boolean;
  autofocus = false;
  toolbarOptions = ['Cancel','Search'];
  filterSettings = { type: 'Excel' };
  subject = new Subject<InkScanner>();
  subscription: Subscription[] = [];

  fieldsBuildings: object = { text: 'name', value: 'id' };
  buildingID = 0;
  buildingName = '';
  toggleColor = true;
  startDate: Date;
  endDate: Date;
  buildings: any;
  model: InInk = {} as InInk;
  public editSettings = {
    showDeleteConfirmDialog: false,
    allowEditing: true,
    allowAdding: true,
    allowDeleting: true,
    mode: "Normal",
  };
  readonly: boolean = true
  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: InInkService,
    private trans: TranslateService,
    private serviceInk: InkService,
    private cdr: ChangeDetectorRef
  ) {
    super(trans);
  }
  ngAfterViewInit(): void {
    this.cdr.detectChanges();
  }
  ngOnDestroy(): void {
    this.subscription.forEach(item => item.unsubscribe());
  }

  public ngOnInit(): void {
    this.getInk();
    this.endDate = new Date();
    this.startDate = new Date();
    // this.getBuildingWorker(() => {
    //   this.buildingID = +localStorage.getItem('buildingID');
    //   if (this.buildingID === 0) {
    //     this.alertify.warning('Vui lòng chọn tòa nhà trước!', true);
    //   }
    // });
    this.getAllInk();
    this.checkQRCode();
  }
  actionBegin(args) {

    if (args.requestType === "beginEdit" ) {
      this.model = {...args.rowData};

      console.log(this.model);
      
    }
    if (args.requestType === "save" ) {
      if (args.action === "edit") {
        this.model.deliver = args.data.deliver || 0;
        // this.modelSup.ProcessID = args.data.processID;
        this.update(this.model);
      }
    }
  
  }

  update(modelSup) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.service.update(modelSup as InInk).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.getInk();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (error) => {
            this.alertify.warning(this.alert.system_error_msg);
          }
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getInk();
      }
    );
  
  }
  onClickDefault() {
    this.startDate = new Date();
    this.endDate = new Date();
    this.getAllIngredientInfoByBuilding()
  }

  startDateOnchange(args) {
    this.startDate = (args.value as Date);
    this.getAllIngredientInfoByBuilding()
  }

  endDateOnchange(args) {
    this.endDate = (args.value as Date);
    this.getAllIngredientInfoByBuilding()
  }
  getBuildingWorker(callback) {
    const userID = +JSON.parse(localStorage.getItem('user')).user.id;
    // this.authService.getBuildingUserByUserID(userID).subscribe((res) => {
    //   this.buildings = res.data;
    //   callback();
    // });
  }
  getBuilding(callback): void {
    // this.buildingService.getBuildings().subscribe(async (buildingData) => {
    //   this.buildings = buildingData.filter(item => item.level === BUILDING_LEVEL);
    //   callback();
    // });
  }
  onFilteringBuilding: EmitType<FilteringEventArgs> = (
    e: FilteringEventArgs
  ) => {
    let query: Query = new Query();
    // frame the query based on search string with filter type.
    query =
      e.text !== '' ? query.where('name', 'contains', e.text, true) : query;
    // pass the filter data source, filter query to updateData method.
    e.updateData(this.buildings as any, query as any);
  }
  onChangeBuilding(args) {
    localStorage.setItem('buildingID', this.buildingID + '');
    this.buildingID = args.itemData.id;
    this.buildingName = args.itemData.name;
    this.getAllIngredientInfoByBuilding();

  }

  NO(index) {
    return (this.ingredientinfoGrid.pageSettings.currentPage - 1) * this.ingredientinfoGrid.pageSettings.pageSize + Number(index) + 1;
  }

  dataBound() {
    // this.ingredientinfoGrid.autoFitColumns();
  }

  OutputChange(args) {
    this.checkin = false;
    this.checkout = true;
    // this.qrcodeChange = null ;
    this.getAllIngredientInfoByBuilding();
  }

  InputChange(args) {
    this.checkin = true;
    this.checkout = false;
    this.getAllIngredientInfoByBuilding();
    // this.qrcodeChange = null ;
  }

  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      case 'PDF Export':
        break;
    }
  }

  showPopupWindow(count, chemical) {
    this.alertify.$swal.fire({
      html: `<div class='d-flex justify-content-center align-items-center' style='Width:100%; height: 400px;'>
               <h1 style='font-size: 150px;' class='display-1 mb-3 align-self-center text-${this.toggleColor === true ? 'success' : 'danger'} font-weight-bold'> ${count} | ${chemical.name}</h1>
             </div>`,
      timer: 2000,
      showConfirmButton: false,
      timerProgressBar: true,
      width: '90%',
      icon: 'success'
    });
    this.toggleColor = !this.toggleColor;
  }

  private checkQRCode() {
    this.subscription.push(this.subject
      .pipe(debounceTime(500))
      .subscribe(async (res) => {
        const input = res.QRCode.split('-') || [];
        // const dateAndBatch = /(\d+)-(\w+)-([\w\-\d]+)/g;
        const dateAndBatch = /(\d+)-(\w+)-/g;
        const validFormat = res.QRCode.match(dateAndBatch);
        const qrcode = res.QRCode.replace(validFormat[0], '');
        const levels = [1, 0];
        const building = JSON.parse(localStorage.getItem('building'));
        let buildingName = building.name;
        if (levels.includes(building.level)) {
          buildingName = 'E';
        }
        const ink = this.findIngredientCode(qrcode);
        if (this.checkCode === true) {
          this.model.qrCode = ink.code
          this.service.add(this.model).subscribe((res: any) => {
            this.getInk();
            const count = this.findInputedIngredient(qrcode);
            this.showPopupWindow(count, ink);
          })
        } else {
          this.alertify.error(this.trans.instant('Wrong Ink'));
        }
      }));
  }

  // sau khi scan input thay doi
  async onNgModelChangeScanQRCode(args) {
    const scanner: InkScanner = {
      QRCode: args,
      ink: null
    };
    this.subject.next(scanner);
    // if (this.buildingID === 0) {
    //   this.alertify.warning('Vui lòng chọn tòa nhà trước!', true);
    // } else {
      
    // }
  }

  // load danh sach IngredientInfo
  getInk() {
    this.service.getAll().subscribe((res: any) => {
      this.data = res;
      console.log(res);
      // this.ConvertClass(res);
    });
  }

  getIngredientInfoOutput() {
    // this.ingredientService.getAllIngredientInfoOutput().subscribe((res: any) => {
    //   this.data = res;
    // });
  }

  getAllIngredientInfoByBuilding() {
    // this.ingredientService.getAllIngredientInfoByBuilding(this.buildingName,this.startDate.toDateString(), this.endDate.toDateString()).subscribe((res: any) => {
    //   this.data = res;
    // });
  }

  getAllIngredientInfoOutputByBuilding() {
    // this.ingredientService.getAllIngredientInfoOutputByBuilding(this.buildingName,this.startDate.toDateString(), this.endDate.toDateString()).subscribe((res: any) => {
    //   this.data = res;
    //   // this.ConvertClass(res);
    // });
  }

  // tim Qrcode dang scan co ton tai khong
  findIngredientCode(code) {
    for (const item of this.ink) {
      if (item.code === code) {
        // return true;
        this.checkCode = true;
        return item;
      } else {
        this.checkCode = false;
      }
    }
  }

  findInputedIngredient(code) {
    let count = this.data?.filter((item: any) => item.code === code && item.status === false).length;
    return count = count === 0 ? 1 : count + 1;
  }

  findOutputedIngredient(code) {
    let count = this.data.filter((item: any) => item.code === code && item.status === true).length;
    return count = count === 0 ? 1 : count + 1;
  }

  // lay toan bo Ingredient
  getAllInk() {
    this.serviceInk.getAll().subscribe((res: any) => {
      this.ink = res;
    });
  }

  // dung de convert color input khi scan nhung chua can dung
  ConvertClass(res) {
    if (res.length !== 0) {
      this.test = 'form-control success-scan';
    } else {
      this.test = 'form-control error-scan';
      this.alertify.error('Wrong Chemical!');
    }
  }

  // xoa Ingredient Receiving
  delete(item) {
    // this.ingredientService.deleteIngredientInfo(item.id, item.code, item.qty, item.batch).subscribe(() => {
    //   this.alertify.success('Delete Success!');
    //   this.getAllIngredientInfoByBuilding()
    //   // this.getIngredientInfo();
    //   // this.getIngredientInfoOutput();
    // });
  }

  // luu du lieu sau khi scan Qrcode vao IngredientReport
  confirm() {
    this.alertify.confirm('Do you want confirm this', 'Do you want confirm this', () => {
      this.alertify.success('Confirm Success');
    });
  }
}
