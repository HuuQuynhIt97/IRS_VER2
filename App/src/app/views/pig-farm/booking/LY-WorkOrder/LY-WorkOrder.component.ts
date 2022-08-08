import { WorkOrder } from './../../../../_core/_model/booking/workOrder';
import { WorkorderService } from './../../../../_core/_service/booking/workorder.service';
import { SiteScreen } from './../../../../_core/_model/club-management/site';
import { Hall } from './../../../../_core/_model/club-management/hall';
import { HallService } from './../../../../_core/_service/club-management/hall.service';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Area, FarmScreen } from 'src/app/_core/_model/farms';
import { AreaService, BarnService, FarmService } from 'src/app/_core/_service/farms';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-LY-WorkOrder',
  templateUrl: './LY-WorkOrder.component.html',
  styleUrls: ['./LY-WorkOrder.component.scss']
})
export class LYWorkOrderComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectArea = new EventEmitter();
  data:DataManager;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  model: WorkOrder;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  subscription: Subscription;
  site: SiteScreen;
  rowIndex: any;
  hallData: any;
  hallFields: object = { text: 'hallName', value: 'guid' };
  siteId: string;
  constructor(
    private service: WorkorderService,
    private serviceHall: HallService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    public translate: TranslateService,
  ) { super(translate);  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  change(args: any) {
    this.pageSettings.currentPage=args.value;
}
  ngOnInit() {
    // this.Permission(this.route);
    let lang = localStorage.getItem('lang');
    let languages = JSON.parse(localStorage.getItem('languages'));
    this.siteId = localStorage.getItem('farmGuid') ;
    setCulture(lang);
    let load = {
      [lang]: {
        grid: languages['grid'],
        pager: languages['pager']
      }
    };
    L10n.load(load);
    this.loadData();
  }
  loadData() {
    this.data = new DataManager({
      url: `${this.baseUrl}WorkOrder/LoadData`,
      adaptor: new UrlAdaptor,
    });
  }
  loadHallData() {
    this.serviceHall.loadDataBySiteGuiId(this.siteId).subscribe(res => {
      this.hallData = res
    })
  }
  ngModelChange(value) {
    this.model.hallGuid = value || "";
  }
  typeChange(value) {
    // this.model.type = value || "";
  }
  // life cycle ejs-grid
  rowSelected(args) {
    this.rowIndex = args.rowIndex;

  }
  recordClick(args: any) {
    this.service.changeHall(args.rowData);
    // this.serviceBarn.changeBarn({} as any);
   }
 dataBound() {
  // this.grid.selectRow(this.rowIndex);
  // this.grid.autoFitColumns();
}

  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      case 'grid_add':
        args.cancel = true;
        this.model = {} as any;
        this.openModal(this.templateRef);
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
        this.service.add(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadData();
              this.modalReference.dismiss();

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
        this.service.update(this.model as WorkOrder).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
              this.modalReference.dismiss();
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
    this.loadHallData();
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      // item.startDate = new Date(item.startDate);
      // item.endDate = new Date(item.endDate);
      this.model = {...item};
      this.getAudit(this.model.id);
      this.title = 'WORKORDER_EDIT_MODAL';
    } else {
      this.model.id = 0;
      // this.model.startDate = new Date();
      // this.model.endDate = new Date(2099,11,31);
      this.title = 'WORKORDER_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }

}
