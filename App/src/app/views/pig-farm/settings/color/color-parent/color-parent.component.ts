import { ColorService } from './../../../../../_core/_service/setting/color.service';
import { ShoeGlueService } from './../../../../../_core/_service/setting/shoe-glue.service';
import { ShoeService } from './../../../../../_core/_service/setting/shoe.service';
import { Shoe } from './../../../../../_core/_model/setting/shoe';
import { GlueService } from './../../../../../_core/_service/setting/glue.service';
import { Glue } from './../../../../../_core/_model/setting/glue';
import { DataManager, UrlAdaptor, Query } from '@syncfusion/ej2-data';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, Input, OnDestroy, OnInit, Output, QueryList, SimpleChanges, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent, QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
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
import { Color } from 'src/app/_core/_model/setting/color';
import { Tooltip } from '@syncfusion/ej2-angular-popups';

@Component({
  selector: 'app-color-parent',
  templateUrl: './color-parent.component.html',
  styleUrls: ['./color-parent.component.scss']
})
export class ColorParentComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectFarm = new EventEmitter();
  data:DataManager;
  baseUrl = environment.apiUrl;

  password = '';
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  model: Color = {} as Color;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  public dateValue: Date = new Date();
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  @ViewChild('importModal') templateRefImportModal: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  groupCode: any;
  rowIndex: number;
  file: any;
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE;
  loading = 0;
  // pageSettingsMenu: any
  @ViewChildren('tooltip') tooltip: QueryList<any>;
  pageSettingsMenu = { 
    pageCount: 5, 
    pageSizes: [5, 10, 15, "All"],
    pageSize: 5 } as any;
  @Input() message: string;
  public query: Query ;
  constructor(
    private service: ColorService,
    private serviceShoeGlue: ShoeGlueService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    private utilityService: UtilitiesService,
    public translate: TranslateService,
  ) {
    super(translate);
    
  }

  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log('ngOnChanges');
  }
  
  ngOnInit() {
    console.log('ngOnInit');
    this.loadDataAsync()
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
   
    // this.service.changeGlue({} as Glue);
  }
  
  async loadDataAsync() {
    await this.loadData();
    // await this.getMenuPageSetting()
  }
  tooltips(args){ 
    if(args.requestType !== undefined) {
    }
    const model = { guid : args.data.guid};
    this.service.getToolTip(model).subscribe((res: any) => {
      if(res.length > 0) {
        let tooltip: Tooltip = new Tooltip({ 
          content: res.join('<br>'),
          position: 'TopLeft'
         }, args.cell); 
      }else {
        let tooltip: Tooltip = new Tooltip({ 
          content: 'N/A',
          position: 'TopLeft',
         }, args.cell); 
      }
      
    });
  //  let tooltip: Tooltip = new Tooltip({ 
  //   content: args.data['name'].toString()
  //  }, args.cell); 
  } 
  
  onBeforeRender(args, data, i) {
    const t = this.tooltip.filter((item, index) => index === +i)[0];
    t.content = 'Loading...';
    t.dataBind();
    const model = { guid : data.guid};
    this.service.getToolTip(model).subscribe((res: any) => {
      t.content = res.join('<br>');
      t.dataBind();
    });
  }
  async loadData() {
    return new Promise((res, rej) => {
      this.data = new DataManager({
        url: `${this.baseUrl}Color/LoadData`,
        adaptor: new UrlAdaptor,
      });
      res(this.data);
    })
  }
  async getMenuPageSetting() {
    return new Promise((res, rej) => {
      this.serviceShoeGlue.getMenuPageSetting().subscribe(
        (result) => {
          this.pageSettingsMenu = {
            pageCount: result.pageCount,
            pageSize: result.pageSize,
            pageSizes: result.pageSizes
          }
          this.pageSettingsMenu?.pageSizes.unshift(['All'])
          
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
    // this.serviceShoeGlue.getMenuPageSetting().subscribe(res => {
    //   this.pageSettingsMenu = {
    //     pageCount: res.pageCount,
    //     pageSize: res.pageSize,
    //     pageSizes: res.pageSizes
    //   }
    //   this.pageSettingsMenu?.pageSizes.unshift(['All'])
    // })
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
    this.service.changeColor(args.rowData);
 }

  dataBound() {
    this.grid.selectRow(this.rowIndex);
    // this.grid.autoFitColumns();
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
      this.service
      .import(this.file)
      .subscribe((res: any) => {
        this.loadData();
        this.modalReference.close();
        this.alertify.success(MessageConstants.CREATED_OK_MSG);
      });
    }, () => {
    });
  }
  toolbarClick(args) {
    console.log(args.item.id);
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
              this.alertify.warning(this.translate.instant(res.message),true);
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
         this.service.update(this.model as Color).subscribe(
           (res) => {
             if (res.success === true) {
               this.alertify.success(this.alert.updated_ok_msg);
               this.loadData();
               this.modalReference.dismiss();
             } else {
               this.alertify.warning(this.translate.instant(res.message),true);
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
      this.title = 'COLOR_EDIT_MODAL';
    } else {
      this.model.id = 0;
      // this.model.version = '1'
      // this.model.startDate = new Date();
      // this.model.endDate = new Date(2099,11,31);
      this.title = 'COLOR_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }

  async openImportModal(template) {
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
