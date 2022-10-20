import { DataManager, UrlAdaptor, Query } from '@syncfusion/ej2-data';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';

import { EmitType, setCulture, L10n } from '@syncfusion/ej2-base';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { FilteringEventArgs, highlightSearch } from '@syncfusion/ej2-angular-dropdowns';
import { ColorWorkPlanService } from 'src/app/_core/_service/execution/color-work-plan.service';
import { ColorWorkPlan } from 'src/app/_core/_model/execution/color-work-plan';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-color-mixing-todo',
  templateUrl: './color-mixing-todo.component.html',
  styleUrls: ['./color-mixing-todo.component.scss']
})
export class ColorMixingTodoComponent extends BaseComponent implements OnInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';


  baseUrl = environment.apiUrl;
  password = '';
  modalReference: NgbModalRef;
  pageSettingss = { pageCount: 20, pageSizes: true, pageSize: 20 };

  @ViewChild('grid') public grid: GridComponent;


  dataColorToDo: any;
  colorToDo: any;

  currentTime: any;

  user = JSON.parse(localStorage.getItem('user')).id;
  setFocus: any;
  locale ;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: false, allowDeleting: false, mode: 'Normal' };
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
    private spinner: NgxSpinnerService,
  ) {
	    super(translate);
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }
  }

  ngOnInit() {
    this.toolbarOptions = ['Search'];
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

    this.loadColorToDo();
    this.colorToDo ={
    };

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
    if (args.requestType === 'save') {
      if (args.action === 'edit') {
        this.colorToDo.id = args.data.id;
        this.colorToDo.executeAmount = args.data.executeAmount;
        this.update();
      }
    }
  }



  actionComplete(args) {
  }

  // end life cycle ejs-grid

  // api



  loadColorToDo() {

    this.serviceColorWorkPlan.loadColorToDo().subscribe((res) => {
      this.dataColorToDo = res
    })
  }



  update() {
    this.serviceColorWorkPlan.updateColorToDoAmount(this.colorToDo).subscribe(
      (res) => {
        if (res.success === true) {
          this.alertify.success(this.alert.updated_ok_msg);
          this.loadColorToDo();
        } else {
          this.alertify.warning(this.alert.updated_failed_msg);
        }
      },
      (error) => {
        this.alertify.warning(this.alert.system_error_msg);
      }
    );
  }
  ngModelChange(value) {
  }
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }




  updateIsFinishedColorToDo(Id: any) {
    this.colorToDo.id = Id;
    this.colorToDo.finishedBy = this.user;
    this.colorToDo.finishedTime = this.datePipe.transform(new Date(), 'yyyy-MM-ddThh:mm:ss') //use datePipe instead .toISOString();

    this.serviceColorWorkPlan.updateIsFinishedColorToDo(this.colorToDo).subscribe((res) => {
      if (res.success === true) {
        this.alertify.success(this.alert.updated_ok_msg);
        this.loadColorToDo();
      } else {
        this.alertify.warning(this.alert.updated_failed_msg);
      }
    },
    (error) => {
      this.alertify.warning(this.alert.system_error_msg);
    })
    this.colorToDo = {};
  }

}
