import { DataManager, UrlAdaptor, Query } from '@syncfusion/ej2-data';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { CodePermission } from 'src/app/_core/_model/code-permission';
import { EmitType, setCulture, L10n } from '@syncfusion/ej2-base';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { FilteringEventArgs, highlightSearch } from '@syncfusion/ej2-angular-dropdowns';
import { ColorWorkPlanService } from 'src/app/_core/_service/execution/color-work-plan.service';
import { ColorWorkPlan } from 'src/app/_core/_model/execution/color-work-plan';


@Component({
  selector: 'app-color-mixing-work-plan',
  templateUrl: './color-mixing-work-plan.component.html',
  styleUrls: ['./color-mixing-work-plan.component.css']
})
export class ColorMixingWorkPlanComponent extends BaseComponent implements OnInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';

  dataColorWorkPlan: DataManager;
  baseUrl = environment.apiUrl;
  password = '';
  modalReference: NgbModalRef;
  pageSettingss = { pageCount: 20, pageSizes: true, pageSize: 20 };

  @ViewChild('grid') public grid: GridComponent;

  colorWPModel: ColorWorkPlan;
  dataShoe: any = [];
  shoeFields: object = {text: 'name', value: 'guid'};
  currentTime: any;
  user = JSON.parse(localStorage.getItem('user')).id;
  setFocus: any;
  locale ;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  fields: object = { text: 'name', value: 'id', itemCreated: (e: any) => {
    highlightSearch(e.item, this.queryString, true, 'Contains');
} };
  parentData: any = [];
  @ViewChild('optionModal') templateRef: TemplateRef<any>;

public onFiltering =  (e: FilteringEventArgs) => {
   // take text for highlight the character in list items.
   this.queryString = e.text;
   let query: Query = new Query();
   query = (e.text !== '') ? query.where('name', 'contains', e.text, true) : query;
   e.updateData(this.parentData, query);
};
  queryString: string;
  constructor(

    private serviceColorWorkPlan: ColorWorkPlanService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
     private config: NgbTooltipConfig,
    public translate: TranslateService,
  ) {
	    super(translate);
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }
  }

  ngOnInit() {
    this.toolbarOptions = ['Add', 'Search'];
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
    this.loadDataColorWorkPlan();
    this.getAllShoes();

  }
  // life cycle ejs-grid
  headerCellInfo(args) {
    // if (this.isAdmin) {
    //   const toolcontent = args.cell.column.headerText;
    //   const field = args.cell.column.field;
    //   const tooltip: Tooltip = new Tooltip({
    //     content: this.keys[field]
    //  });
    //  tooltip.appendTo(args.node);
    // }
 }

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  onChange(args, data) {
    data.isDefault = args.checked;

  }

  actionBegin(args) {
  }


// life cycle ejs-grid
toolbarClick(args) {

      switch (args.item.id) {
        case 'grid_add':
          args.cancel = true;
          this.colorWPModel = {} as any;
          this.openModal(this.templateRef);
          break;
        default:
          break;
      }
  }
  actionComplete(args) {
  }

  // end life cycle ejs-grid

  // api

  loadDataColorWorkPlan() {
    const accessToken = localStorage.getItem('token');
    this.dataColorWorkPlan = new DataManager({
      url: `${this.baseUrl}ColorWorkPlan/LoadDataColorWorkPlan`,
      adaptor: new UrlAdaptor,
      headers: [{ authorization: `Bearer ${accessToken}` }]
    });
  }
  getAllShoes() {
    this.serviceColorWorkPlan.getAllShoes().subscribe((res) => {
      this.dataShoe = res
    })
  }

  delete(id) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.deleteTitle,
      this.alert.deleteMessage,
      () => {
        this.serviceColorWorkPlan.deleteColorWorkPlan(id).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.deleted_ok_msg);
              this.loadDataColorWorkPlan();
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
        this.colorWPModel.shoeGuid = this.colorWPModel.shoeGuid
        this.colorWPModel.executeDate = this.datePipe.transform(this.colorWPModel.executeDate, 'MM/dd/yyyy')
        // this.colorWPModel.executeDate = this.datePipe.transform(this.colorWPModel.executeDate, 'MM/dd/yyyy HH:mm:ss')

        this.serviceColorWorkPlan.addColorWorkPlan(this.colorWPModel).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadDataColorWorkPlan();
              this.modalReference.dismiss();
            } else {
              this.alertify.warning(this.alert.created_failed_msg);
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
  update() {
   this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
      this.colorWPModel.executeDate = this.datePipe.transform(this.colorWPModel.executeDate, 'MM/dd/yyyy')

    this.serviceColorWorkPlan.updateColorWorkPlan(this.colorWPModel).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(this.alert.updated_ok_msg);
          this.loadDataColorWorkPlan();
          this.modalReference.dismiss();
        } else {
          this.alertify.warning(this.alert.updated_failed_msg);
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
  ngModelChange(value) {
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }
  save() {
    if (this.colorWPModel.id > 0) {
      this.update();
    } else {
      this.create();
    }
  }
 async openModal(template, data = {} as ColorWorkPlan) {
    this.colorWPModel = {...data};
    if (this.colorWPModel.id > 0) {
      this.title = 'COLOR_WORK_PLAN_EDIT_MODAL';
      // this.currentTime = this.datePipe.transform(this.colorWPModel.executeDate, 'dd/MM/yyyy');
      // this.getAudit(this.colorWPModel.id);

    } else {
      this.title = 'COLOR_WORK_PLAN_ADD_MODAL';
      this.colorWPModel.id = 0;
      this.currentTime = new Date();
      this.colorWPModel.executeDate = this.datePipe.transform(this.currentTime, 'MM/dd/yyyy')
      this.colorWPModel.shoeGuid = this.colorWPModel.shoeGuid
      // this.colorWPModel.executeDate = this.datePipe.transform(this.currentTime, 'MM/dd/yyyy HH:mm:ss')
      // this.colorWPModel.createDate = this.datePipe.transform(this.currentTime, 'dd/MM/yyyy HH:mm:ss');
      this.colorWPModel.createBy = this.user;

    }
    this.modalReference = this.modalService.open(template, { size: 'xl' });
  }

}
