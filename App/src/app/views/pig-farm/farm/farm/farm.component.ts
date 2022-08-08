import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Farm } from 'src/app/_core/_model/farms';
import { AreaService, BarnService, FarmService, PenService } from 'src/app/_core/_service/farms';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-farm',
  templateUrl: './farm.component.html',
  styleUrls: ['./farm.component.css']
})
export class FarmComponent extends BaseComponent implements OnInit {
  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectFarm = new EventEmitter();
  data:DataManager;
  baseUrl = environment.apiUrl;

  password = '';
  modalReference: NgbModalRef;
  pageSettings = { pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  model: Farm;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  groupCode: any;
  rowIndex: number;
  constructor(
    private service: FarmService,
    private servicePen: PenService,
    private serviceBarn: BarnService,
    private serviceArea: AreaService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    public translate: TranslateService,
  ) { super(translate);  }

  ngOnInit() {
    // this.Permission(this.route);
    this.groupCode = JSON.parse(localStorage.getItem('user')).groupCode || "";
    if (this.groupCode !== 'ADMIN') {
      this.toolbarOptions = ['Search'];
    }

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
    this.service.changeFarm({} as Farm);
    this.loadData();
  }
  typeChange(value) {
    this.model.type = value || "";
  }
  // life cycle ejs-grid
  rowSelected(args) {
    this.rowIndex = args.rowIndex;
  }
  recordClick(args: any) {
    this.service.changeFarm(args.rowData);
    this.serviceBarn.changeBarn({} as any);
    this.serviceArea.changeArea({} as any);
    this.servicePen.changePen({} as any);
 }

  dataBound() {
    this.grid.selectRow(this.rowIndex);
    this.grid.autoFitColumns();
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
  loadData() {
      const accessToken = localStorage.getItem('token');
      const farmGuid = localStorage.getItem('farmGuid');
      this.data = new DataManager({
        url: `${this.baseUrl}Farm/LoadData?farmGuid=${farmGuid}`,
        adaptor: new UrlAdaptor,
        headers: [{ authorization: `Bearer ${accessToken}` }]
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
        this.model.startDate = this.ToDate(this.model.startDate);
        this.model.endDate = this.ToDate(this.model.endDate);
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
    if (date &&  date instanceof Date ) {
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
        this.model.startDate = this.ToDate(this.model.startDate);
        this.model.endDate = this.ToDate(this.model.endDate);
        this.service.update(this.model as Farm).subscribe(
          (res) => {
            if (res.success === true) {
              this.modalReference.dismiss();
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
 async openModal(template, data = {} as Farm) {
    this.model = {} as Farm;
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      item.startDate =item.startDate ? new Date(item.startDate) : null;
      item.endDate = item.endDate ? new Date(item.endDate): null;
      this.model = {...item};
      this.getAudit(this.model.id);
      this.title = 'FARM_EDIT_MODAL';
    } else {
      this.model.id = 0;
      this.model.startDate = new Date();
      this.model.endDate = new Date(2099,11,31);
      this.title = 'FARM_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }
}
