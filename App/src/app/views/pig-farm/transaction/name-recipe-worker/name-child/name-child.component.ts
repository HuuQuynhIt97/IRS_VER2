import { ScheduleService } from './../../../../../_core/_service/transaction/schedule.service';
import { ShoeGlueService } from './../../../../../_core/_service/setting/shoe-glue.service';
import { ShoeService } from './../../../../../_core/_service/setting/shoe.service';
import { Shoe } from './../../../../../_core/_model/setting/shoe';
import { GlueService } from './../../../../../_core/_service/setting/glue.service';
import { Glue } from './../../../../../_core/_model/setting/glue';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ImagePathConstants, MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Farm } from 'src/app/_core/_model/farms';
import { AreaService, BarnService, FarmService, PenService } from 'src/app/_core/_service/farms';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { UtilitiesService } from 'src/app/_core/_service/utilities.service';
import { Site } from 'src/app/_core/_model/club-management/site';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { ProcessService } from 'src/app/_core/_service/setting/process.service';
import { Process2Service } from 'src/app/_core/_service/setting/process2.service';

@Component({
  selector: 'app-name-child',
  templateUrl: './name-child.component.html',
  styleUrls: ['./name-child.component.scss']
})
export class NameChildComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectFarm = new EventEmitter();
  data:DataManager;
  baseUrl = environment.apiUrl;
  password = '';
  modalReference: NgbModalRef;
  pageSettingsMenu: any;
  @ViewChild('grid') public grid: GridComponent;
  model: Shoe = {} as Shoe;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  public dateValue: Date = new Date();
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add','Cancel','Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  groupCode: any;
  rowIndex: number;
  file: any;
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE;
  loading = 0;
  ChemicalFields: object = { text: 'name', value: 'guid' };
  dataTreatment: any;
  dataProcess: any;
  @ViewChild('importModal') templateRefImportModal: TemplateRef<any>;
  constructor(
    private service: ShoeService,
    private serviceSchedule: ScheduleService,
    private serviceShoeGlue: ShoeGlueService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private serviceTreatment: ProcessService,
    private utilityService: UtilitiesService,
    private serviceProcess: Process2Service,
    public translate: TranslateService,
  ) {
    super(translate);
    this.getMenuPageSetting()
  }
  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnInit() {
    // this.Permission(this.route);
    this.groupCode = JSON.parse(localStorage.getItem('user')).groupCode || "";
    if (this.groupCode !== 'ADMIN' && this.groupCode !== 'Development Center') {
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
    this.loadData();
    this.loadDataTreatment();
    this.loadDataProcess();
    // this.service.changeGlue({} as Glue);
  }

  fileProgress(event) {
    this.file = event.target.files[0];
  }

  uploadFile() {
    if (this.file === null) {
      this.alertify.error('Please choose file upload ! ');
      return;
    }
    this.alertify.confirm2('Warning! <br>!', 'uploading this !', () => {
      this.serviceSchedule.import(this.file)
      .subscribe((res: any) => {
        this.loadData();
        this.modalReference.close();
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
      });
    }, () => {
    });
  }
  async openImportModal(template) {
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }
  loadDataTreatment() {
    this.serviceTreatment.getAll().subscribe((res: any) => {
      this.dataTreatment = res
    })
  }

  loadDataProcess() {
    this.serviceProcess.getAll().subscribe((res: any) => {
      this.dataProcess = res
    })
  }
  getMenuPageSetting() {
    this.serviceShoeGlue.getMenuPageSetting().subscribe(res => {
      this.pageSettingsMenu = {
        pageCount: res.pageCount,
        pageSize: res.pageSize,
        pageSizes: res.pageSizes
      }
      this.pageSettingsMenu?.pageSizes.unshift(['All'])
    })
  }

  onFileChangeLogo(args) {
    this.file = args.target.files[0];
  }

  typeChange(value) {
    // this.model.type = value || "";
  }

  // life cycle ejs-grid
  rowSelected(args) {
    this.rowIndex = args.rowIndex;
  }

  recordClick(args: any) {
    this.service.changeShoe(args.rowData);
  }

  dataBound() {
    this.grid.selectRow(this.rowIndex);
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
      case 'grid_Import Excel':
        args.cancel = true;
        this.openImportModal(this.templateRefImportModal);
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
    this.data = new DataManager({
      url: `${this.baseUrl}Shoe/LoadData`,
      adaptor: new UrlAdaptor,
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
        this.model.productionDate = this.datePipe.transform(this.dateValue, "yyyy/MM/dd")
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
         this.service.update(this.model as Shoe).subscribe(
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
  async openModal(template, data = {} as Glue) {
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      this.model = {...item};
      this.getAudit(this.model.id);
      this.title = 'SHOE_EDIT_MODAL';
    } else {
      this.model.id = 0;
      // this.model.startDate = new Date();
      // this.model.endDate = new Date(2099,11,31);
      this.title = 'SHOE_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }

  imagePath(path) {
    if (path !== null && this.utilityService.checkValidImage(path)) {
      if (this.utilityService.checkExistHost(path)) {
        return path;
      }
      return this.apiHost + path;
    }
    return this.noImage;
  }

}
