import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { Process2Service } from './../../../../../_core/_service/setting/process2.service';
import { ProcessService } from './../../../../../_core/_service/setting/process.service';
import { ColorService } from 'src/app/_core/_service/setting/color.service';
import { TreatmentWayService } from './../../../../../_core/_service/setting/treatment-way.service';
import { PartService } from './../../../../../_core/_service/setting/part.service';
import { ScheduleService } from './../../../../../_core/_service/transaction/schedule.service';
import { Schedule } from './../../../../../_core/_model/transaction/schedule';
import { ShoeGlueService } from './../../../../../_core/_service/setting/shoe-glue.service';
import { ShoeGlue } from './../../../../../_core/_model/setting/shoeGlue';
import { ShoeService } from './../../../../../_core/_service/setting/shoe.service';
import { ChemicalService } from './../../../../../_core/_service/setting/chemical.service';
import { Chemical } from './../../../../../_core/_model/setting/chemical';
import { GlueService } from './../../../../../_core/_service/setting/glue.service';

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
import { ShoeScreen } from 'src/app/_core/_model/setting/shoe';

@Component({
  selector: 'app-recipe-child',
  templateUrl: './recipe-child.component.html',
  styleUrls: ['./recipe-child.component.css']
})
export class RecipeChildComponent extends BaseComponent implements OnInit, OnDestroy {

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
  schedule: Schedule = {} as Schedule
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add','Update','Cancel', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  subscription: Subscription;
  glue: ShoeScreen;
  rowIndex: any;
  public dataPosition: object[] = [
    { id: '1', name: 'A' },
    { id: '2', name: 'B' },
    { id: '3', name: 'C' },
    { id: '4', name: 'D' },
    { id: '5', name: 'E' },
  ];
  public fieldsPosition: object = { text: 'name', value: 'name' };
  ChemicalFields: object = { text: 'name', value: 'guid' };
  groupCode: any;
  chemicalEditSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  dataGlue: any;
  dataPart: any;
  dataTreatment: any;
  dataTreatmentWay: any;
  dataTreatmentWay2: any;
  dataProcess: any;
  dataColor: any;
  pageSettingsRecipe: any
  constructor(
    private service: ScheduleService,
    private serviceShoe: ShoeService,
    private servicePart: PartService,
    private serviceTreatmentWay: TreatmentWayService,
    private serviceColor: ColorService,
    private serviceTreatment: ProcessService,
    private serviceProcess: Process2Service,
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
    this.subscription = this.serviceShoe.currentShoe.subscribe(shoe => {
      this.schedule.shoesGuid = shoe.guid
      this.schedule.treatmentGuid = shoe.treatmentGuid
      this.glue = shoe
      if (this.glue?.guid) {
        this.rowIndex = undefined;
        this.loadData();
        this.loadDataPart();
        this.loadDataColor();
        this.loadDataTreatment();
        // this.loadDataTreatmentWay();
        this.getAllTreatmentWay();
        this.loadDataProcess();
      }
    });
    // this.loadData();
  }

  getRecipePageSetting() {
    // this.service.getRecipePageSetting().subscribe(res => {
    //   this.pageSettingsRecipe = {
    //     pageCount: res.pageCount,
    //     pageSize: res.pageSize,
    //     pageSizes: res.pageSizes
    //   }
    //   this.pageSettingsRecipe?.pageSizes.unshift(['All'])
    // })
  }
  loadData() {
    this.service.loadData(this.schedule.shoesGuid,this.locale).subscribe((res: any) => {
      this.data = res
    })
  }
  loadDataPart() {
    // this.dataPart = new DataManager({
    //   url: `${this.baseUrl}Part/LoadData`,
    //   adaptor: new UrlAdaptor,
    // }).dataSource;
    this.servicePart.loadDataByLang(this.locale).subscribe((res: any) => {
      this.dataPart = res
    })
  }
  loadDataTreatmentWay() {
    this.serviceTreatmentWay.getAll().subscribe((res: any) => {
      this.dataTreatmentWay = res.map(item => {
        return {
          name: item.name + '(' + item.process + ')',
          guid: item.guid
        }
      })
    })
  }

  getAllTreatmentWay() {
    this.serviceTreatmentWay.getAllTreatmentWay(this.schedule.treatmentGuid).subscribe((res: any) => {
      this.dataTreatmentWay = res.map(item => {
        return {
          name: item.name + '(' + item.process + ')',
          guid: item.guid
        }
      })
    })
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
  loadDataColor() {
    this.serviceColor.getAll().subscribe((res: any) => {
      this.dataColor = res
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
    this.schedule.id = data.id
    this.schedule.colorGuid = data.colorGuid
    this.schedule.treatmentWayGuid = data.treatmentwayGuid
    this.schedule.partGuid = data.partGuid
  }
  initialModel() {
    this.schedule.colorGuid = ''
    this.schedule.treatmentWayGuid = ''
    this.schedule.partGuid = ''
    this.schedule.id = 0
  }
  async actionBegin(args) {
    if (args.requestType === 'add') {
      this.initialModel();
    }

    if (args.requestType === 'beginEdit') {
      const item = await this.getById(args.rowData.id);
      this.schedule = {...item};
    }

    if (args.requestType === 'save' && args.action === 'add') {
      if (this.schedule.partGuid === '') {
        this.alertify.warning('Please select Glue!', true);
        args.cancel = true;
        return;
      }
      this.schedule.consumption = args.data.consumption
      this.create();
    }

    if (args.requestType === 'save' && args.action === 'edit') {
      this.schedule.consumption = args.data.consumption
      this.update();
    }

    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }

  recordClick(args: any) {
    // this.service.changeHall(args.rowData);
    // this.serviceBarn.changeBarn({} as any);
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
        if (!this.glue?.guid) {
          this.translate.get('Please select a Glue!').subscribe(data => {
            args.cancel = true;
            this.alertify.warning(data, true);
           })
           return;
         }
        // args.cancel = true;
        // this.model = {} as any;
        // this.openModal(this.templateRef);
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
        this.service.add(this.schedule).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
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
        this.service.update(this.schedule as Schedule).subscribe(
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
   if (!this.glue?.guid) {
    this.translate.get('Please select a Site!').subscribe(data => {
      this.alertify.warning(data, true);
     })
     return;
   }
    if (data?.id > 0) {
      const item = await this.getById(data.id);
      this.schedule = {...item};
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
