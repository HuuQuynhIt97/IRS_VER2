import { DataManager, UrlAdaptor, Query } from '@syncfusion/ej2-data';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild, TemplateRef, Output, EventEmitter, Input, SimpleChanges, OnChanges, ChangeDetectionStrategy, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ExcelExportProperties, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { Dashboard } from 'src/app/_core/_model/dashboard';
import { DashboardService } from 'src/app/_core/_service/dashboard.service';
import { setCulture, L10n } from '@syncfusion/ej2-base';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { FilteringEventArgs, highlightSearch } from '@syncfusion/ej2-angular-dropdowns';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-setting-dashboard-chart',
  templateUrl: './setting-dashboard-chart.component.html',
  styleUrls: ['./setting-dashboard-chart.component.scss']
})
export class SettingDashboardChartComponent extends BaseComponent implements OnInit, OnChanges, OnDestroy {
  @Output() systemMenuGuid = new EventEmitter();
  @Input() topHeight;
  dashboardGuid = '';
  areaGuid= '';
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  data: DataManager;
  baseUrl = environment.apiUrl;
  password = '';
  modalReference: NgbModalRef;
  
  @ViewChild('grid') public grid: GridComponent;
  model: Dashboard;
  setFocus: any;
  locale ;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  fields: object = { text: 'name', value: 'id', itemCreated: (e: any) => {
    highlightSearch(e.item, this.queryString, true, 'Contains');
} };
  parentData: any = [];
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = [];
public onFiltering =  (e: FilteringEventArgs) => {
   // take text for highlight the character in list items.
   this.queryString = e.text;
   let query: Query = new Query();
   query = (e.text !== '') ? query.where('name', 'contains', e.text, true) : query;
   e.updateData(this.parentData, query);
};
  queryString: string;
  commonRules:object;
  nameRules: object;
  requireNameRules: object;
  value = 'value';
 public nameFn: (args: { [key: string]: string }) => boolean = (args: { [key: string]: string }) => {
  return args[this.value].length <= 255;
}

public commonFn: (args: { [key: string]: string }) => boolean = (args: { [key: string]: string }) => {
  return args[this.value].length <= 25;
}
  codeTypeData: any[];

  queryString2: string;
  @ViewChild('parentTemplate', {static:true})
  public parentTemplate: any;
  public onFiltering2 =  (e: FilteringEventArgs) => {
    // take text for highlight the character in list items.
    this.queryString2 = e.text;
    let query: Query = new Query();
    query = (e.text !== '') ? query.where('name', 'contains', e.text, true) : query;
    e.updateData(this.codeTypeData, query);
  };
  codeTypeItem = "";
  valueColor01= "#000000";
  valueColor02= "#000000";
  valueColor03= "#000000";
  valueColor04= "#000000";
  valueColor05= "#000000";
  valueColor06= "#000000";
  valueColor07= "#000000";
  valueColor08= "#000000";
  valueColor09= "#000000";
  valueColor10= "#000000";
  textColor01= "#ffffff";
  subscriptions: Subscription[] =  [];
  constructor(
    private service: DashboardService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
     private config: NgbTooltipConfig,
    public translate: TranslateService,
    private cdr: ChangeDetectorRef,
  ) {
	    super(translate);
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach(x=> x.unsubscribe())
  }
  ngOnChanges(changes: SimpleChanges): void {

    // if (this.areaGuid != changes?.areaGuid?.currentValue) {
    //   this.areaGuid = changes.areaGuid.currentValue;
    //   this.loadData();

    // }
    // if (this.topHeight != changes.topHeight.currentValue) {
    //   this.topHeight = changes.topHeight.currentValue;
    // }
  }
  ngOnInit() {
    this.cdr.detach();
    setInterval(() => {
      this.cdr.detectChanges();
    }, 100);
    this.toolbarOptions =['Add', 'Search'];
    this.codeTypeItem = "All";

    this.commonRules = { required: [true, this.validationGrid.requireField], maxLength: [this.commonFn, `${this.validationGrid.textLength} <= 25`] };
    this.nameRules = { maxLength: [this.nameFn, `${this.validationGrid.textLength} <= 255`] };
    this.requireNameRules = { required: [true, this.validationGrid.requireField], maxLength: [this.nameFn, `${this.validationGrid.textLength} <= 255`] };
    let lang = localStorage.getItem('lang');
    this.locale = lang;
    let languages = JSON.parse(localStorage.getItem('languages'));
    setCulture(lang);
    let load = {
      [lang]: {
        grid: languages['grid'],
        pager: languages['pager']
      }
    };
    L10n.load(load);
    this.subscriptions.push(this.service.currentDashboard.subscribe( guid => {
      this.dashboardGuid = guid;
      if (this.dashboardGuid && this.areaGuid) {
        this.loadData();
      }
    }));
    this.subscriptions.push(this.service.currentArea.subscribe( guid => {
      this.areaGuid = guid;
      if (this.dashboardGuid && this.areaGuid) {
        this.loadData();
      }
    }));

  }
  // life cycle ejs-grid
  headerCellInfo(args) {
 }
  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }
  rowSelected(args) {
    this.systemMenuGuid.emit(args.data.guid);
  }
  actionBegin(args) {
    if (args.requestType === 'add') {
      this.valueColor01 = '#000000';
      this.valueColor02 = '#000000';
    }
    if (args.requestType === 'beginEdit') {
      const data = args.rowData;
      this.valueColor01 = data.valueColor01 || '#000000';
      this.valueColor02 = data.valueColor02 || '#000000';
    }
    if (args.requestType === 'save' && args.action === 'add') {
      this.model = {...args.data};
      this.model.id = 0;
      this.model.upperDashBoard = this.dashboardGuid;
      this.model.upperArea = this.areaGuid;
      this.model.type = 'E';
      this.create(() => {});

    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.model = {...args.data};
      this.update(() => {});
    }
  }

loadLang() {
  this.translate.get('Code Type').subscribe( functionName => {
    this.functionName = functionName;
  });
   this.translate.get('Print by').subscribe(printBy => {
    this.printBy = printBy;
  });
}
// life cycle ejs-grid
toolbarClick(args) {
  const functionName = this.functionName;
  const printBy = this.printBy;
      switch (args.item.id) {
        case 'grid_excelexport':
          const accountName = JSON.parse(localStorage.getItem('user'))?.accountName || 'N/A';
          const header = {
            headerRows: 3,
            rows: [
              {
                cells: [{
                    colSpan: 5, value: `* ${functionName}`,
                    style: { fontColor: '#fd7e14', fontSize: 18, hAlign: 'Left', bold: true, }
                }]
            },
            {
              cells: [{
                  colSpan: 5, value: `* ${this.datePipe.transform(new Date(), 'yyyyMMdd_HHmmss')}`,
                  style: { fontColor: '#fd7e14', fontSize: 18, hAlign: 'Left', bold: true, }
              }]
          },
          {
            cells: [{
                colSpan: 5, value: `* ${printBy} ${accountName}`,
                style: { fontColor: '#fd7e14', fontSize: 18, hAlign: 'Left', bold: true, }
            }]
        },
            ]
          } as any;

          const fileName = `${functionName}_${this.datePipe.transform(new Date(), 'yyyyMMdd_HHmmss')}.xlsx`
          const excelExportProperties: ExcelExportProperties = {
            hierarchyExportMode: 'All',
            fileName: fileName,
            header: header
        };
          this.grid.excelExport(excelExportProperties);
          break;
        case 'grid_add':
          // args.cancel = true;
          // this.model = {} as any;
          // this.openModal(this.templateRef);
          break;
        default:
          break;
      }
}
  actionComplete(args) {
    if (args.requestType === 'beginEdit') {
      args.form.elements.namedItem(this.setFocus.field).focus();
    }
  }

  // end life cycle ejs-grid
  onChange(args) {
    this.codeTypeItem = args.itemData.id;
    this.loadData();
  }
  // api
  loadData() {
    const accessToken = localStorage.getItem('token');
    let query = `&type=E`;
    this.data = new DataManager({
      url: `${this.baseUrl}Dashboard/LoadItemData?dashboardGuid=${this.dashboardGuid}&areaGuid=${this.areaGuid}${query}`,
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
  create(callBack) {

    this.service.add(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          callBack();
          this.alertify.success(this.alert.created_ok_msg);
          this.loadData();
        } else {
          this.alertify.warning(this.alert.system_error_msg);
        }
      },
      (error) => {
        const message = this.translate.instant(error) || this.alert.system_error_msg;
        this.alertify.warning(message);
      }
    );
  }
  update(callBack) {
    this.service.update(this.model).subscribe(
      (res) => {
        if (res.success === true) {
          callBack();
          this.alertify.success(this.alert.updated_ok_msg);
          this.loadData();

        } else {
          this.alertify.warning(this.alert.system_error_msg);
        }
      },
      (error) => {
        const message = this.translate.instant(error) || this.alert.system_error_msg;
        this.alertify.warning(message);
      }
    );
  }
  ngModelChange(value) {
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }
  save() {
    if (this.model.id > 0) {
      this.update(() => {});
    } else {
      this.create(() => {});
    }
  }
  onChangeColor(data,value) {
    data.color = value;
  }
}
