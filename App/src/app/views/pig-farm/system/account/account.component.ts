import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { EmployeeService } from './../../../../_core/_service/employee.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Component, OnInit, TemplateRef, ViewChild, AfterViewInit } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, GridComponent, ExcelExportProperties, ExcelExportCompleteArgs } from '@syncfusion/ej2-angular-grids';
import { NgbModal, NgbModalRef, NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';
import { ActivatedRoute } from '@angular/router';
import { XAccount } from 'src/app/_core/_model/xaccount';
import { XAccountGroup } from 'src/app/_core/_model/xaccount-group';
import { ActionConstant, ImagePathConstants, MessageConstants } from 'src/app/_core/_constants';
import { XAccountGroupService } from 'src/app/_core/_service/xaccount-group.service';
import { AccountService } from 'src/app/_core/_service/account.service';
import { AccountTypeService } from 'src/app/_core/_service/account-type.service';
import { CheckBoxSelectionService, FilteringEventArgs } from '@syncfusion/ej2-angular-dropdowns';
import { DataManager, Query, UrlAdaptor } from '@syncfusion/ej2-data';
import { environment } from 'src/environments/environment';
import { UtilitiesService } from 'src/app/_core/_service/utilities.service';
import { EmitType, setCulture, L10n } from '@syncfusion/ej2-base';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { FarmService } from 'src/app/_core/_service/farms';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';


declare let $: any;

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss'],
  providers: [ToolbarService, EditService, PageService, CheckBoxSelectionService, NgxSpinnerService]
})
export class AccountComponent extends BaseComponent implements OnInit, AfterViewInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  data: DataManager;
  password = '';
  modalReference: NgbModalRef;
  fields = { text: 'name', value: 'codeNo' };
  // toolbarOptions = ['Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;

  @ViewChild('grid') public grid: GridComponent;
  accountCreate: XAccount;
  accountUpdate: XAccount;
  setFocus: any;
  locale = localStorage.getItem('lang');
  xaccountGroupData: XAccountGroup[];
  xaccountGroupItem: any;
  xAccounts: any[];
  accountTypeFields: object = { text: 'name', value: 'guid' };
  siteFields: object = { text: 'siteName', value: 'guid' };
  employeeFields: object = { text: 'nickName', value: 'guid' };
  xaccountGroupFields: object = { text: 'groupName', value: 'guid' };
  accountTypeId: any;
  accountTypes: any[];
  employeeData: any[];
  employeeGuid = null;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  @ViewChild('odsTemplate', {static:true}) public odsTemplate: any;
  xaccountGroup: any;
  ocid: any;
  ocidList: any;
  status = false;
  permissionData: [] = [];
  permissions: any;
  searchOptions = { fields: ['farmName', 'accountNo', 'accountName', 'uid', 'employeeNickName', 'accountGroupName'], operator: 'contains', ignoreCase: true };
  public onFiltering: any = (e: FilteringEventArgs) => {
    let query = new Query();
    //frame the query based on search string with filter type.
    query = (e.text != "") ? query.where("name", "contains ", e.text, true) : query;
    //pass the filter data source, filter query to updateData method.
    e.updateData(this.permissionData, query);
  };
  model: XAccount;
  file: any;
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE;
  previewImg: any;
  accountGuid = "";
  loading = 0;
  baseUrl = environment.apiUrl;
  siteData:any;
  constructor(
    private service: XAccountService,
    private serviceAccountType: AccountTypeService,
    private serviceEmployee: EmployeeService,
    private xaccountGroupService: XAccountGroupService,
    public modalService: NgbModal,
    private serviceFarm: FarmService,
    private alertify: AlertifyService,
    private utilityService: UtilitiesService,
    private route: ActivatedRoute,
    private datePipe: DatePipe,
     private config: NgbTooltipConfig,
    public translate: TranslateService,
    private siteService: SiteService
  ) {
	    super(translate);
      if (this.isAdmin === false) {
        config.disableTooltip = true;
      }
  }
  ngAfterViewInit(): void {
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
    this.getSites();
    this.loadData();
    this.loadXAccountGroupData();
    this.loadLang();
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

  async togglePassword() {
    // if (this.model?.accountId > 0) {
    //   try {
    //     const data = await this.service.showPassword(this.model?.accountId).toPromise();
    //     this.status =! this.status;
    //     this.model.upwd = this.status ? data.passwordDecrypt :  data.passwordEncrypt;
    //   } catch { }
    // } else { this.status =! this.status; }
    this.status =! this.status;
  }
  getEmployeesByXAccountID(xAccountID) {
    this.serviceEmployee.getEmployeesByAccountID(xAccountID || 0).subscribe(data => {
      this.employeeData = data;
    });
  }

  getSites() {
    this.siteService.getAll().subscribe(data => {
      this.siteData = data;
    });
  }

  getXAccountType() {
    this.serviceAccountType.getAll().subscribe(data => {
      this.accountTypes = data;
    });
  }
  // life cycle ejs-grid

  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }
  initialModel() {
    this.accountTypeId = 0;
    this.xaccountGroup = null;
    this.employeeGuid = null;
    this.ocidList = [];
    this.accountCreate = {} as XAccount;

  }
  updateModel(data) {
    this.accountTypeId = data.xAccountTypeId;
    this.xaccountGroup = data.xaccountGroup;
    this.employeeGuid = data.employeeGuid;
    this.ocidList = data.ocidList;
  }
  xXAccountGroupName(id) {
    return this.xaccountGroupData.filter(x => x.id == id)[0]?.groupName || "N/A";
  }
  siteName(id) {
    return this.siteData.filter(x => x.id == id)[0]?.name || "N/A";
  }
  employeeName(id) {
    return this.employeeData.filter(x => x.id == id)[0]?.nickName || "N/A";
  }
  actionBegin(args) {
    if (args.requestType === 'add') {
      this.initialModel();
    }
    if (args.requestType === 'beginEdit') {
      const item = args.rowData;
      this.updateModel(item);
    }
    if (args.requestType === 'save' && args.action === 'add') {
      this.accountCreate = { ...args.data };
      this.accountCreate.accountId = 0;
      this.accountCreate.accountGroup = this.xaccountGroup;

      if (args.data.username === undefined) {
        this.alertify.error('Please key in a xAccount!');
        args.cancel = true;
        return;
      }
      if (args.data.password === undefined) {
        this.alertify.error('Please key in a password!');
        args.cancel = true;
        return;
      }

      this.create();
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.accountUpdate = { ...args.data };
      this.accountUpdate.employeeGuid = this.employeeGuid;
      this.accountUpdate.accountGroup = this.xaccountGroup;
      this.update();
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }

  ngModelChange(value) {
    this.model.employeeGuid = value || "";
  }
  valueChange(value) {
    this.model.accountGroup = value || "";
  }
  siteValueChange(value) {
    this.model.farmGuid = value || [];
  }

loadLang() {
  this.translate.get('Account').subscribe( functionName => {
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
    // if (args.requestType === 'add') {
    //   args.form.elements.namedItem('username').focus(); // Set focus to the Target element
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
    const farmGuid = localStorage.getItem('farmGuid');
    this.data = new DataManager({
      url: `${this.baseUrl}XAccount/LoadData?farmGuid=${farmGuid}`,
      adaptor: new UrlAdaptor,
      headers: [{ authorization: `Bearer ${accessToken}` }]
    });
  }

  loadXAccountGroupData() {
    this.xaccountGroupService.getAccountGroup().subscribe((data: any) => {
      this.xaccountGroupData = data;
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
          (err) => this.alertify.error(err, true)
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
        this.model.file = this.file || [];
        delete this.model['column'];
        delete this.model['index'];
        this.service.insertForm(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadData();
              this.getEmployeesByXAccountID(0);

              this.modalReference.dismiss();
            } else {
              this.translate.get(res.message).subscribe((data: string) => {
                this.alertify.warning(data, true);
              });
            }

          },
          (error) => {
            this.alertify.warning(this.alert.system_error_msg);
          }
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.loadData();

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
        this.model.file = this.file || [];
        delete this.model['column'];
        delete this.model['index'];
        this.service.updateForm(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
              this.getEmployeesByXAccountID(0);

              this.modalReference.dismiss();
            } else {
              this.alertify.warning(res.message, true);
            }
          },
          (error) => {
            this.alertify.warning(this.alert.system_error_msg);
          }
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.loadData();

      }
    );

  }
  storePermiison() {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      'Store Permission',
      'Are you sure you want to store this?',
      () => {
        const request = {
          guid: this.accountGuid,
          permissions: this.permissions
        }
        this.service.storePermission(request).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success("Successfully!");
              this.modalReference.dismiss();
            } else {
              this.alertify.warning(res.message, true);
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

    if (this.model.accountId > 0) {
      this.update();
    } else {
      this.create();
    }
  }
  preview(template, data) {
    this.previewImg = this.imagePath(data.photoPath);
    this.modalService.open(template, { size: 'xl', backdrop: 'static' });
  }
  async openPermissionModal(template, data = {} as XAccount) {
    this.loading = 1;
    this.accountGuid = data.guid;
    const guid = this.accountGuid || "";
    const lang = localStorage.getItem('lang');
    this.permissionData = [];
    this.permissions = [];
    this.modalReference = this.modalService.open(template, { size: 'xl', backdrop: 'static' });
    try {
      const permissionData = await this.service.getPermissionsDropdown(guid,lang).toPromise();
      this.permissionData = permissionData;
      const permissions = await this.service.getPermissions(guid,lang).toPromise();
      this.permissions = permissions || [];
      this.loading = 0;
    } catch (error) {
      this.loading = 0;
    }

  }
  async openModal(template, data = {} as XAccount) {
    this.loading = 1;

    if (data?.accountId > 0) {
      this.model = {...data};
      this.model.farmGuid = data.farmGuid;
      this.model.accountGroup = data.accountGroup;
      this.model.employeeGuid = data.employeeGuid;
      this.getAudit(this.model.accountId);
      this.title = 'ACCOUNT_EDIT_MODAL'
    } else {
      this.title = 'ACCOUNT_ADD_MODAL';
      this.model.accountId = 0;
    }
    this.modalReference = this.modalService.open(template, { size: 'xl', backdrop: 'static' });
   
    const option = {
      overwriteInitial: true,
      maxFileSize: 1500,
      showClose: false,
      showCaption: false,
      browseLabel: '',
      removeLabel: '',
      browseIcon: '<i class="bi-folder2-open"></i>',
      removeIcon: '<i class="bi-x-lg"></i>',
      removeTitle: 'Cancel or reset changes',
      elErrorContainer: '#kv-avatar-errors-1',
      msgErrorClass: 'alert alert-block alert-danger',
      defaultPreviewContent: '<img src="../../../../../assets/images/no-img.jpg" alt="No Image">',
      layoutTemplates: { main2: '{preview} ' + ' {browse}' },
      allowedFileExtensions: ["jpg", "png", "gif"],
      initialPreview: [],
      initialPreviewConfig: [],
      deleteUrl: `${environment.apiUrl}XAccount/DeleteUploadFile`
    };
    if (this.model.photoPath) {
      this.model.photoPath = this.imagePath(this.model.photoPath);
      const img = `<img src='${this.model.photoPath}' class='file-preview-image' alt='Desert' title='Desert'>`;
      option.initialPreview = [img]

      const a = {
        caption: '',
        width: '',
        url: `${environment.apiUrl}XAccount/DeleteUploadFile`, // server delete action
        key: this.model.accountId,
        extra: { id: this.model.accountId }
      }
      option.initialPreviewConfig = [a];
    }
    $("#avatar-1").fileinput(option);;
    let that = this;
    $('#avatar-1').on('filedeleted', function (event, key, jqXHR, data) {
      console.log('Key = ' + key);
      that.file = null;
      that.model.file = null;
      that.model.photoPath = null;
      option.initialPreview = [];
      option.initialPreviewConfig = [];
      $(this).fileinput(option);

    });
    try {
      const employees = await this.serviceEmployee.getEmployeesByAccountID(data?.accountId || 0).toPromise();
      this.employeeData = employees;
      this.loading = 0;
      if (data?.accountId > 0) {
        this.model.employeeGuid = data.employeeGuid;
      }

    } catch (error) {
    this.loading = 0;

    }

  }
  onFileChangeLogo(args) {
    this.file = args.target.files[0];
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


