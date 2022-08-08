import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { EditService, ExcelExportCompleteArgs, ExcelExportProperties, GridComponent, SelectionService } from '@syncfusion/ej2-angular-grids';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EmployeeService } from 'src/app/_core/_service/employee.service';
import { environment } from 'src/environments/environment';
import { DisplayTextModel } from '@syncfusion/ej2-angular-barcode-generator';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Employee } from 'src/app/_core/_model/employee';
import { MessageConstants } from 'src/app/_core/_constants';
import { setCulture, L10n } from '@syncfusion/ej2-base';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';

declare let window:any;

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css'],
  providers: [SelectionService, EditService]
})
export class EmployeeComponent extends BaseComponent implements OnInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  localLang =  window.navigator.userLanguage || window.navigator.language;
  data: DataManager;

  @ViewChild('grid') public grid: GridComponent;
  excelDownloadUrl: string;
  modalReference: NgbModalRef;
  selectOptions = { persistSelection: true };
  selectedData: Employee[] = [];
  displayTextMethod: DisplayTextModel = {
    visibility: false
  };
  model: Employee;
  editModel: Employee;
  baseUrl = environment.apiUrl;
  selectedRowIndex = undefined;
  checkedAll = false;
  sortSettings = { columns: [{ field: 'id', direction: 'Descending' }] };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  @ViewChild('odsTemplate', {static:true}) public odsTemplate: any;
  index: any;
  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: EmployeeService,
    private spinner: NgxSpinnerService,
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
    this.toolbarOptions = ['ExcelExport',{template: this.odsTemplate}, 'Add', 'Search'];
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
    // this.Permission(this.route);
    this.excelDownloadUrl = `${environment.apiUrl}Employee/ExcelExport`;
    this.loadData();
    this.loadLang();
  }

  getAudit(id) {
    this.service.getAudit(id).subscribe(data => {
      this.audit = data;
    });

  }
  loadData() {
    const accessToken = localStorage.getItem('token');
    this.data = new DataManager({
      url: `${this.baseUrl}Employee/LoadDataByFarm?farmGuid=${localStorage.getItem('farmGuid')}&lang=${this.globalLang}`,
      insertUrl: `${this.baseUrl}Employee/Add`,
      updateUrl: `${this.baseUrl}Employee/Update`,
      adaptor: new UrlAdaptor,
      headers: [{ authorization: `Bearer ${accessToken}` }]
    });
  }

loadLang() {
  this.translate.get('Employee').subscribe( functionName => {
    this.functionName = functionName;
  });
   this.translate.get('Print by').subscribe(printBy => {
    this.printBy = printBy;
  });
}
sexChange(value) { this.model.sex = value; }
unitChange(value) { this.model.unit = value; }
deptChange(value) { this.model.dept = value; }
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
          args.cancel = true;
          this.model = {} as any;
          this.openModal(this.templateRef);
          break;
        default:
          break;
      }
  }

  actionComplete(args: any) {
    if (args.requestType === 'add') {
    }
    if (args.requestType === 'beginEdit') {

    }
    if (args.requestType === 'save') {
      if (args.action == 'add') {
        args.promise.then(x=> {
          if (x.statusCode == 200) {
            this.alertify.success(this.alert.created_ok_msg);
            this.model = {} as Employee;
            this.modalReference.dismiss();
          } else {
            this.alertify.error(x.message, true);
          }
        })

      } else if (args.action == 'edit') {
        this.editModel = {} as Employee;
        this.alertify.success(this.alert.updated_ok_msg);
        this.modalReference.dismiss();
      }
    }
  }
  actionBegin(args) {
    if (args.requestType === 'searching') {
      this.checkedAll = false;
      this.grid.pageSettings.pageSize = 12;
      this.selectedData = [];
    }
    if (args.requestType === 'beginEdit') {
      const data = args.rowData;
    }
    if (args.requestType === 'save' && args.action === 'add') {
      this.model = { ...args.data };

      if (args.data.nickName === undefined) {
        this.alertify.error('Please key in a nickName!');
        args.cancel = true;
        return;
      }
      if (args.data.no === undefined) {
        this.alertify.error('Please key in a NO!');
        args.cancel = true;
        return;
      }
      if (args.data.name === undefined) {
        this.alertify.error('Please key in a name!');
        args.cancel = true;
        return;
      }
      let birthDay = this.datePipe.transform(new Date(), 'yyyy/MM/dd');
      if (args.data.birthDay instanceof Date) {
        birthDay = this.datePipe.transform(args.data.birthDay, 'yyyy/MM/dd');
      } else {
        birthDay = args.data.birthDay;
      }
      this.model.birthDay = birthDay;

      args.data = this.model;
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.editModel = { ...args.data };
      let birthDay = this.datePipe.transform(new Date(), 'yyyy/MM/dd');
      if (args.data.birthDay instanceof Date) {
        birthDay = this.datePipe.transform(args.data.birthDay, 'yyyy/MM/dd');
      } else {
        birthDay = args.data.birthDay;
      }
      this.editModel.birthDay = birthDay;
      args.data = this.editModel;
    }
    if (args.requestType === 'delete') {
      //this.delete(args.data[0].id);
    }
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


  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }
  save() {
    if (this.model.id > 0) {
      this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
        'Update record',
        'Are you sure you want to update this record?',
        () => {
          this.grid.updateRow(this.index, this.model);
        }, () => {
          this.alertify.error(this.alert.cancelMessage);
        }
      );

    } else {
      this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
        'Add record',
        'Are you sure you want to add this record?',
        () => {
          this.model.farmGuid = localStorage.getItem('farmGuid');
          this.grid.addRecord(this.model, 0);
        }, () => {
          this.alertify.error(this.alert.cancelMessage);
        }
      );

    }
  }
  openModal(template, data = {} as Employee, index = null) {
    this.model = {...data};
    this.model.birthDay = data.birthDay || new Date();
    this.index = index || null;
    if (this.model.id > 0) {
      this.title = 'EMPLOYEE_EDIT_MODAL';
      this.getAudit(this.model.id);

    } else {
      this.model.id = 0;
      this.title = 'EMPLOYEE_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, { size: 'xl',backdrop: 'static' });
  }
  odsExport() {
    const functionName = this.functionName;
    const printBy = this.printBy;
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

          const fileName = `${functionName}_${this.datePipe.transform(new Date(), 'yyyyMMdd_HHmmss')}.ods`
          const excelExportProperties: ExcelExportProperties = {
            hierarchyExportMode: 'All',
            fileName: fileName,
            header: header
        };

    this.isodsExport = true;

    this.grid.excelExport(excelExportProperties, null, null, true);
  }
  excelExpComplete(args: ExcelExportCompleteArgs) {
    if (this.isodsExport) {
      const fileName = `${this.functionName}_${this.datePipe.transform(new Date(), 'yyyyMMdd_HHmmss')}.ods`

      args.promise.then((e: { blobData: Blob }) => {
        const model = {
          functionName: fileName,
          file: e.blobData
        }
        this.service.downloadODSFile(model).subscribe((res: any) => {
        this.service.downloadBlob(res.body, fileName, 'application/vnd.oasis.opendocument.spreadsheet')
        })
      });
    }

  }
}

