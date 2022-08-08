import { DataManager, UrlAdaptor, Query } from '@syncfusion/ej2-data';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ExcelExportCompleteArgs, ExcelExportProperties, GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { SystemLanguage } from 'src/app/_core/_model/system-language';
import { SystemLanguageService } from 'src/app/_core/_service/system-language.service';
import { setCulture, L10n } from '@syncfusion/ej2-base';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { FilteringEventArgs } from '@syncfusion/ej2-angular-dropdowns';
declare let $;
@Component({
  selector: 'app-system-language',
  templateUrl: './system-language.component.html',
  styleUrls: ['./system-language.component.css']
})
export class SystemLanguageComponent extends BaseComponent implements OnInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  data: DataManager;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;

  @ViewChild('grid') public grid: GridComponent;
  model: SystemLanguage;
  setFocus: any;
  locale;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = [];

  pageData: any = [];
  typeData: any = [];
  @ViewChild('pageTemplate', { static: true }) pageTemplate: any;
  @ViewChild('typeTemplate', { static: true }) typeTemplate: any;

  public onFiltering = (e: FilteringEventArgs) => {
    // take text for highlight the character in list items.
    this.queryString = e.text;
    let query: Query = new Query();
    query = (e.text !== '') ? query.where('contains', e.text, true) : query;
    e.updateData(this.pageData, query);
  };
  queryString: string;
  toobarParentData = [];

  queryString2: string;
  @ViewChild('parentTemplate', { static: true })
  public parentTemplate: any;
  public onFiltering2 = (e: FilteringEventArgs) => {
    // take text for highlight the character in list items.
    this.queryString2 = e.text;
    let query: Query = new Query();
    query = (e.text !== '') ? query.where('contains', e.text, true) : query;
    e.updateData(this.typeData, query);
  };
  slpage: string = "";
  sltype: string = "";
  @ViewChild('odsTemplate', {static:true}) public odsTemplate: any;
  constructor(
    private service: SystemLanguageService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    config: NgbTooltipConfig,
    public translate: TranslateService
  ) {
    super(translate);
    if (this.isAdmin === false) {
      config.disableTooltip = true;
    }
  }
  ngOnInit() {

    this.toolbarOptions = ['ExcelExport',{template: this.odsTemplate}, 'Add',
      { template: this.pageTemplate }, 'Search'];

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
    this.loadPageData();
    this.loadTypeData();
    this.loadData();
    this.loadLang();
  }
  // life cycle ejs-grid
  headerCellInfo(args) {
  }
  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  onChangePage(value) {
    this.loadData();
  }
  onChangeType(args) {
    this.loadData();
  }

  actionBegin(args) {
  }

  loadLang() {
    this.translate.get('System Language').subscribe(functionName => {
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
        args.cancel = true;
        this.model = {} as any;
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
  getAudit(id) {
    this.service.getAudit(id).subscribe(data => {
      this.audit = data;
    });

  }
  loadPageData() {
    this.pageData = [];
    this.service.getPages(localStorage.getItem('lang')).subscribe(data => {
      this.pageData = data;
      this.pageData.unshift('All');
    });

  }
  loadTypeData() {
    this.pageData = [];
    this.service.getTypes(localStorage.getItem('lang')).subscribe(data => {
      this.typeData = data;
      this.typeData.unshift('All');
    });

  }
  loadData() {
    const accessToken = localStorage.getItem('token');
    this.data = new DataManager({
      url: `${this.baseUrl}SystemLanguage/LoadData?page=${this.slpage}&type=${this.sltype}`,
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

        this.service.add(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadData();
              this.modalReference.dismiss();
            } else {
              this.alertify.warning(res.message, true);
            }
          },
          (error) => {
            if (error.indexOf('error') == -1) {
              this.translate.get(error).subscribe((res: string) => {
                this.alertify.warning(res, true);
              });
            } else {
              this.alertify.warning(this.alert.serverError, true);

            }
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

        this.service.update(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
              this.modalReference.dismiss();
            } else {
              this.alertify.warning(res.message, true);
            }
          },
          (error) => {
            if (error.indexOf('error') == -1) {
              this.translate.get(this.alert.exist_message).subscribe((res: string) => {
                this.alertify.warning(res, true);
              });
            } else {
              this.alertify.warning(this.alert.serverError, true);

            }
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
  openModal(template, data = {} as SystemLanguage) {
    this.model = { ...data };
    if (this.model.id > 0) {
      this.title = 'SYSTEM_LANGUAGE_EDIT_MODAL';
      this.getAudit(this.model.id);
    } else {
      this.title = 'SYSTEM_LANGUAGE_ADD_MODAL';
      this.model.id = 0;

    }
    this.modalReference = this.modalService.open(template, { size: 'xl' });

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


