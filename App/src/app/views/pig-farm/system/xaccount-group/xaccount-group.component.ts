import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ExcelExportCompleteArgs, ExcelExportProperties, GridComponent, QueryCellInfoEventArgs } from '@syncfusion/ej2-angular-grids';
import { Tooltip } from '@syncfusion/ej2-angular-popups';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { XAccountGroup } from 'src/app/_core/_model/xaccount-group';
import { ActionConstant, MessageConstants } from 'src/app/_core/_constants';
import { XAccountGroupService } from 'src/app/_core/_service/xaccount-group.service';
import { EmitType, setCulture, L10n } from '@syncfusion/ej2-base';
import { DatePipe } from '@angular/common';
import { DataManager, Query, UrlAdaptor } from '@syncfusion/ej2-data';
import { FilteringEventArgs } from '@syncfusion/ej2-angular-dropdowns';


@Component({
  selector: 'app-xaccount-group',
  templateUrl: './xaccount-group.component.html',
  styleUrls: ['./xaccount-group.component.scss']
})
export class XAccountGroupComponent extends BaseComponent implements OnInit {
  data: XAccountGroup[] = [];
  password = '';
  modalReference: NgbModalRef;

  @ViewChild('grid') public grid: GridComponent;
  model: XAccountGroup;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  @ViewChild('odsTemplate', {static:true}) public odsTemplate: any;
  loading: number;
  permissions: any[];
  permissionData: any[];
  accountGroupGuid: any;
  fields = { text: 'name', value: 'codeNo' };
  public onFiltering: any = (e: FilteringEventArgs) => {
    let query = new Query();
    //frame the query based on search string with filter type.
    query = (e.text != "") ? query.where("name", "contains ", e.text, true) : query;
    //pass the filter data source, filter query to updateData method.
    e.updateData(this.permissionData, query);
  };
  constructor(
    private service: XAccountGroupService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
    public translate: TranslateService,
  ) { super(translate);  }

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
    this.loadData();
    this.loadLang();
  }
  // life cycle ejs-grid

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  makeAction(input: string): string[] {
    switch (input) {
      case ActionConstant.CREATE:
        this.editSettings.allowAdding = true;
        return ['Add'];
      case ActionConstant.EDIT:
        this.editSettings.allowEditing = false;
        return [];
      case ActionConstant.DELETE:
        this.editSettings.allowDeleting = false;
        return [];
      case ActionConstant.EXCEL_EXPORT:
        return ['ExcelExport'];
      default:
        return [undefined];
    }
  }
  actionBegin(args) {
  }

loadLang() {
  this.translate.get('Account Group').subscribe( functionName => {
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
  loadData() {
    this.service.getAll().subscribe(data => {
      this.data = data;
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
  async openModal(template, data = {} as XAccountGroup) {
    const farmGuid = localStorage.getItem('farmGuid');
    if (data?.id > 0) {
      try {
        this.title = 'ACCOUNT_GROUP_EDIT_MODAL';
        this.model = {...data};
        this.modalReference = this.modalService.open(template, {size: 'xl', backdrop: 'static'});
        this.getAudit(this.model.id);
        this.loading = 1;
        this.accountGroupGuid = data.guid;
        const guid = this.accountGroupGuid || "";
        const lang = localStorage.getItem('lang');
        this.permissionData = [];
        this.permissions = [];
        this.model.permissions = {
          guid: this.accountGroupGuid || "",
          permissions: this.permissions
        }
        const permissionData = await this.service.getPermissionsDropdown(guid,lang).toPromise();
        this.permissionData = permissionData;
        const permissions = await this.service.getPermissions(guid,lang).toPromise();
        this.permissions = permissions || [];
        this.model.permissions = {
          guid: this.accountGroupGuid || "",
          permissions: this.permissions
        }
        this.loading = 0;
      } catch (error) {
        this.loading = 0;
      }

    } else {
      this.title = 'ACCOUNT_GROUP_ADD_MODAL';
      this.loading = 1;
      this.model.id = 0;
      this.model.farmGuid = farmGuid;
      this.accountGroupGuid = this.model.guid;
      this.modalReference = this.modalService.open(template, {size: 'xl', backdrop: 'static'});
      const guid = this.accountGroupGuid || "";
      const lang = localStorage.getItem('lang');
      this.permissionData = [];
      this.permissions = [];
      this.model.permissions = {
        guid: this.accountGroupGuid || "",
        permissions: this.permissions
      }
      try {
        const permissionData = await this.service.getPermissionsDropdown(guid,lang).toPromise();
        this.permissionData = permissionData;
        this.model.permissions = {
          guid: this.accountGroupGuid || "",
          permissions: this.permissions
        }
        this.loading = 0;
      } catch (error) {
        this.loading = 0;
      }

    }
  }
  onChangePermission() {
    this.model.permissions = {
      guid: this.accountGroupGuid || "",
      permissions: this.permissions
    }
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


