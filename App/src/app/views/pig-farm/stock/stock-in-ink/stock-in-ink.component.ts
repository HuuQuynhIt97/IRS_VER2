import { StockInInk } from './../../../../_core/_model/stock/stockInInk';
import { StockInInkService } from './../../../../_core/_service/stock/stock-in-ink.service';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, OnDestroy, Input } from '@angular/core';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { Hall } from 'src/app/_core/_model/club-management/hall';
import { GlueChemical } from 'src/app/_core/_model/setting/glueChemical';
import { InkColorService } from 'src/app/_core/_service/setting/ink-color.service';
import { InkService } from 'src/app/_core/_service/setting/ink.service';
import { ShoeGlueService } from 'src/app/_core/_service/setting/shoe-glue.service';
import { ColorService } from 'src/app/_core/_service/setting/color.service';
import { InkColor } from 'src/app/_core/_model/setting/inkColor';
import { ColorScreen } from 'src/app/_core/_model/setting/color';
import { Chemical } from 'src/app/_core/_model/setting/chemical';

@Component({
  selector: 'app-stock-in-ink',
  templateUrl: './stock-in-ink.component.html',
  styleUrls: ['./stock-in-ink.component.scss']
})
export class StockInInkComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectArea = new EventEmitter();
  data: any;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  model: Hall;
  dataChemical: Chemical = {} as Chemical
  glueChemical: GlueChemical = {} as GlueChemical
  inkColor: StockInInk = {} as StockInInk
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions: any ;
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  subscription: Subscription;
  color: ColorScreen;
  rowIndex: any;
  public dataPosition: object[] = [
    { id: '1', name: 'A' },
    { id: '2', name: 'B' },
    { id: '3', name: 'C' },
    { id: '4', name: 'D' },
    { id: '5', name: 'E' },
  ];
  fiterRequest = {
    createDate: new Date(),
    executionDate: new Date()
  }
  public fieldsPosition: object = { text: 'name', value: 'name' };
  ChemicalFields: object = { text: 'name', value: 'guid' };
  groupCode: any;
  chemicalEditSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  dataInk: any;
  dataSupplier: any;
  pageSettingsRecipe: any
  executeDate: any = new Date();
  insertDate: any = new Date();
  flag: boolean;
  constructor(
    private service: StockInInkService,
    private serviceInk: InkService,
    private serviceShoeGlue: ShoeGlueService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    public translate: TranslateService,
  ) {
    super(translate);
    // this.getRecipePageSetting()
  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  change(args: any) {
    // this.pageSettings.currentPage=args.value;
  }
  ngOnInit() {
    // this.Permission(this.route);
    this.groupCode = JSON.parse(localStorage.getItem('user')).groupCode || "";

    // this.toolbarOptions = ['Add','Execute','Cancel', 'Search'];
    let lang = localStorage.getItem('lang');
    let languages = JSON.parse(localStorage.getItem('languages'));
    setCulture(lang);
    let load = {
      [lang]: {
        grid: languages['grid'],
        pager: languages['pager']
      }
    };
    L10n.load(load);
    this.loadData();
    this.loadDataColor();

    // this.loadData();
  }

  createToolbar() {
    const selectedRecords = this.grid.dataSource as any[];
    const data = selectedRecords.filter(x => x.executeDate === null);
    if(data.length > 0) {
      if (this.groupCode === 'ADMINs') {
        this.toolbarOptions = ['Search'];
      }else {

        this.toolbarOptions = ['Add','Execute','Cancel', 'Search'];
      }
    }else {
      if (this.groupCode === 'ADMINs') {
        this.toolbarOptions = ['Search'];
      }else {

        this.toolbarOptions = ['Add','Cancel', 'Search'];
      }
    }
  }

  onChangeExecuteDate(args) {
    this.loadDataFilter()
  }

  onChangeInsertDate(args) {
    this.loadDataFilter()
  }
  execute() {
    const selectedRecords = this.grid.dataSource as any[];
    const dataUpdate = selectedRecords.filter(x => x.executeDate === null);
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.service.updateExecution(dataUpdate).subscribe(
          (res) => {
            if (res) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
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
      }
    );
  }
  getRecipePageSetting() {
    this.serviceShoeGlue.getRecipePageSetting().subscribe(res => {
      this.pageSettingsRecipe = {
        pageCount: res.pageCount,
        pageSize: res.pageSize,
        pageSizes: res.pageSizes
      }
      this.pageSettingsRecipe?.pageSizes.unshift(['All'])
    })
  }
  loadData() {
    this.service.getAll().subscribe((res: any) => {
      this.data = res
    })
  }

  loadDataFilter() {
    this.service.dataFiterExecuteAndCreate(this.fiterRequest).subscribe((res: any) => {
      this.data = res
    })
  }
  loadDataColor() {
    this.serviceInk.getAll().subscribe((res: any) => {
      this.dataInk = res.map(item => {
        return {
          name: item.name + '(' + item.code + ')' + ' - ' + item.process,
          guid: item.guid
        }
      })
    })
  }
  typeChange(value) {
    this.model.type = value || "";
  }
  onChangePosition(args, data, index) {
    const existValue = this.data.filter(x => x.position === args.value)
    if(existValue.length > 0) {
      this.translate.get('Duplicate position!').subscribe(data => {
        args.cancel = true;
        this.alertify.warning(data, true);
      })
      return;
    }
  }
  onChangeChemical(args, data, index) {

  }
  // life cycle ejs-grid
  rowSelected(args) {
    this.rowIndex = args.rowIndex;

  }
  updateModel(data) {
    this.glueChemical.id = data.id
    this.glueChemical.glueGuid = data.glueGuid
    this.glueChemical.chemicalGuid = data.chemicalGuid
    this.glueChemical.position = data.position
    this.glueChemical.percentage = data.percentage
  }
  initialModel() {
    this.inkColor.inkGuid = ''
    this.inkColor.id = 0
  }
  approveSingle(data) {
    this.service.approve(data.guid).subscribe(res => {
      if (res) {
        this.alertify.success(this.alert.created_ok_msg);
        this.loadData();
        this.createToolbar()
      } else {
        this.alertify.warning(this.alert.system_error_msg);
      }
    })
  }
  unApproveSingle(data) {
    this.service.unApprove(data.guid).subscribe(res => {
      if (res) {
        this.alertify.success(this.alert.created_ok_msg);
        this.loadData();
        this.createToolbar()
      } else {
        this.alertify.warning(this.alert.system_error_msg);
      }
    })
  }
  async actionBegin(args) {
    if (args.requestType === 'add') {
      this.initialModel();
    }

    if (args.requestType === 'beginEdit') {
      if(args.rowData.executeDate !== null) {
        args.cancel = true;
      }
      const item = await this.getById(args.rowData.id);
      this.inkColor = {...item};
    }

    if (args.requestType === 'save' && args.action === 'add') {
      if (this.inkColor.inkGuid === '') {
        this.alertify.warning('Please select Ink!', true);
        args.cancel = true;
        return;
      }
      this.inkColor.realAmount = args.data.realAmount
      this.create();
    }

    if (args.requestType === 'save' && args.action === 'edit') {
      this.inkColor.realAmount = args.data.realAmount
      this.update();
    }

    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }

  recordClick(args: any) {
    this.service.changeHall(args.rowData);
    // this.serviceBarn.changeBarn({} as any);
  }
  dataBound() {
    this.flag = true;
    const selectedRecords = this.grid.dataSource as any[];
    if(selectedRecords !== null) {
      this.createToolbar()
      this.creaOrUpdateColumn()
    }
    // this.grid.selectRow(this.rowIndex);
    // this.grid.autoFitColumns();
  }
  creaOrUpdateColumn() {
    if (!this.flag) { return; }
    if(this.groupCode === 'ADMIN')
    {
      this.grid.hideColumns('approveStatus')
      this.flag = true;
    } else {
      this.grid.showColumns('approveStatus')
      this.grid.hideColumns('ApproveLabel')
    }
  }
  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      case 'grid_Execute':
        this.execute();
        break;
      default:
        break;
    }
  }


  // end life cycle ejs-grid

  // api
  getAudit(id) {
    this.service.getAudit(id).subscribe(data => {
      this.audit = data;
    });
  }

  delete(id) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.deleteTitle,
      this.alert.deleteMessage,
      () => {
        this.service.delete(id).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.deleted_ok_msg);
              this.loadData();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (err) => this.alertify.warning(this.alert.system_error_msg)
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);

      }
    );

  }
  create() {
   this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.createTitle,
      this.alert.createMessage,
      () => {
        this.service.add(this.inkColor).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadData();
              this.createToolbar()
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
      }
    );

  }
  ToDate(date: any) {
    if (date &&  date instanceof Date) {
      return this.datePipe.transform(date, "yyyy/MM/dd");
    } else if (date && !(date instanceof Date)) {
      return date;
    }
     else {
      return null;
    }
  }
  update() {
   this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.service.update(this.inkColor as StockInInk).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
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
      }
    );


  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }

  save() {
    if (this.model.id > 0) {
      this.update();
    } else {
      this.create();
    }
  }
  getById(id) {
   return this.service.getById(id).toPromise();
  }
 async openModal(template, data = {} as Hall) {
   if (!this.color?.guid) {
    this.translate.get('Please select a Site!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      this.inkColor = {...item};
      this.getAudit(this.model.id);
      this.title = 'HALL_EDIT_MODAL';
    } else {
      this.model.id = 0;
      // this.model.startDate = new Date();
      // this.model.endDate = new Date(2099,11,31);
      this.title = 'HALL_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }

}
