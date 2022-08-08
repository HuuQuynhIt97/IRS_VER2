import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { AreaScreen } from './../../../../_core/_model/farms/area';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, OnDestroy, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Pen, FarmScreen, BarnScreen, RoomScreen } from 'src/app/_core/_model/farms';
import { AreaService, PenService, FarmService, BarnService, RoomService } from 'src/app/_core/_service/farms';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
declare let window:any;

@Component({
  selector: 'app-pen',
  templateUrl: './pen.component.html',
  styleUrls: ['./pen.component.css']
})
export class PenComponent extends BaseComponent implements OnInit, OnDestroy {
  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectPen = new EventEmitter();
  data:DataManager;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  model: Pen;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  subscriptions: Subscription[] =[];
  farm: FarmScreen;
  area: AreaScreen;
  barn: BarnScreen;
  room: RoomScreen;
  constructor(
    private service: PenService,
    private serviceRoom: RoomService,
    private serviceBarn: BarnService,
    private serviceArea: AreaService,
    private serviceFarm: FarmService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    public translate: TranslateService,
  ) { super(translate);  }
  ngOnDestroy(): void {
    this.subscriptions?.forEach(item => item.unsubscribe());
  }
  change(args: any) {
    this.pageSettings.currentPage=args.value;
}
  ngOnInit() {
    // this.Permission(this.route);
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
    this.subscriptions.push(this.serviceFarm.currentFarm.subscribe(farm => {
      this.farm = farm
      this.loadData();


    }));
    this.subscriptions.push(this.serviceArea.currentArea.subscribe(area => {
      this.area = area
      this.loadData();

    }));
    this.subscriptions.push(this.serviceBarn.currentBarn.subscribe(barn => {
      this.barn = barn
      this.loadData();

    }));
    this.subscriptions.push(this.serviceRoom.currentRoom.subscribe(room => {
      this.room = room
      this.loadData();

      // if (this.area?.guid && this.farm?.guid && this.barn?.guid && this.room?.guid) {
      //   this.loadData();
      // }
    }));

  }
  dataBound() {
    this.grid.autoFitColumns();
  }
  typeChange(value) {
    this.model.type = value || "";
  }
  // life cycle ejs-grid
  rowSelected(args) {
    //console.log(args.data);
  }
  recordClick(args: any) {
    //console.log(args.rowData);
    this.service.changePen(args.rowData);

 }
  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  onChange(args, data) {
    console.log(args);
    data.isDefault = args.checked;

  }

  actionBegin(args) {
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
  actionComplete(args) {
    // if (args.requestType === 'add') {
    //   args.form.elements.namedItem('name').focus(); // Set focus to the Target element
    // }
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
    this.data = new DataManager({
      url: `${this.baseUrl}Pen/LoadData?farmGuid=${this.farm?.guid || ''}&areaGuid=${this.area?.guid || ''}&barnGuid=${this.barn?.guid || ''}&roomGuid=${this.room?.guid || ''}`,
      adaptor: new UrlAdaptor,
      // headers: [{ authorization: `Bearer ${accessToken}` }]
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
        this.model.farmGuid = this.farm.guid;
        this.model.areaGuid = this.area.guid;
        this.model.barnGuid = this.barn.guid;
        this.model.roomGuid = this.room.guid;
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
        this.model.farmGuid = this.farm.guid;
        this.model.areaGuid = this.area.guid;
        this.model.barnGuid = this.barn.guid;
        this.model.roomGuid = this.room.guid;
        this.model.startDate = this.ToDate(this.model.startDate);
        this.model.endDate = this.ToDate(this.model.endDate);
        this.service.update(this.model as Pen).subscribe(
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
 async openModal(template, data = {} as Pen) {
   if (!this.farm?.guid) {
    this.translate.get('Please select a farm!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
   if (!this.area?.guid) {
    this.translate.get('Please select a area!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
   if (!this.barn?.guid) {
    this.translate.get('Please select a barn!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
   if (!this.room?.guid) {
    this.translate.get('Please select a room!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      item.startDate = new Date(item.startDate);
      item.endDate = new Date(item.endDate);
      this.model = {...item};
      this.getAudit(this.model.id);
      this.title = 'PEN_EDIT_MODAL';
    } else {
      this.model.id = 0;
      this.model.startDate = new Date();
      this.model.endDate = new Date(2099,11,31);
      this.title = 'PEN_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }
}
