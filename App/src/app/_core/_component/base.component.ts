import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from "@angular/router";
import { ActionConstant, MessageConstants } from "../_constants";
import { FunctionSystem } from "../_model/application-user";
import { environment } from 'src/environments/environment';
import { ShoeGlueService } from '../_service/setting/shoe-glue.service';

export abstract class BaseComponent {
  isodsExport = true;
  pageSettings: any;
  pageSettingsColor: any;
  globalLang = localStorage.getItem('lang');
  skip = 0;
  take = 10;
  functionName: string;
  printBy: string;
  audit: any;
  alert = {
    updateMessage: this.translate.instant(MessageConstants.UPDATE_MESSAGE),
    updateTitle: this.translate.instant(MessageConstants.UPDATE_TITLE),
    createMessage:this.translate.instant(MessageConstants.CREATE_MESSAGE),
    createTitle: this.translate.instant(MessageConstants.CREATE_TITLE),
    deleteMessage: this.translate.instant(MessageConstants.DELETE_MESSAGE),
    deleteTitle: this.translate.instant(MessageConstants.DELETE_TITLE),
    cancelMessage: this.translate.instant(MessageConstants.CANCEL_MESSAGE),
    serverError: this.translate.instant(MessageConstants.SERVER_ERROR),
    deleted_ok_msg: this.translate.instant(MessageConstants.DELETED_OK_MSG),
    created_ok_msg: this.translate.instant(MessageConstants.CREATED_OK_MSG),
    created_failed_msg: this.translate.instant(MessageConstants.CREATED_FAILED_MSG),
    updated_ok_msg: this.translate.instant(MessageConstants.UPDATED_OK_MSG),
    updated_failed_msg: this.translate.instant(MessageConstants.UPDATED_FAILED_MSG),
    system_error_msg: this.translate.instant(MessageConstants.SYSTEM_ERROR_MSG),
    exist_message: this.translate.instant(MessageConstants.EXIST_MESSAGE),
    choose_farm_message: this.translate.instant(MessageConstants.CHOOSE_FARM_MESSAGE),
    select_order_message: this.translate.instant(MessageConstants.SELECT_ORDER_MESSAGE),
    yes_message: this.translate.instant(MessageConstants.YES_MSG),
    no_message: this.translate.instant(MessageConstants.NO_MSG),
  };
  validationGrid = {
    requireField:'This field is require.',
    textLength: 'Text Length',
  };
  baseUrl = environment.apiUrl;
  functions: FunctionSystem[];
  editSettingsTree = { allowEditing: false, allowAdding: false, allowDeleting: false, newRowPosition: 'Child', mode: 'Row' };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: false, allowDeleting: false, mode: 'Normal' };
  toolbarOptions = ['ExcelExport', 'Add', 'Edit', 'Delete', 'Cancel', 'Search'] as any[];
  toolbarOptionsTree = [
  'Add',
  'Delete',
  'Search',
  'ExpandAll',
  'CollapseAll',
  'ExcelExport'
  ];
  contextMenuItems = [
    {
      text: 'Add Child',
      iconCss: ' e-icons e-add',
      target: '.e-content',
      id: 'Add-Sub-Item'
    },
    {
      text: 'Delete',
      iconCss: ' e-icons e-delete',
      target: '.e-content',
      id: 'DeleteOC'
    }
  ];

  protected Permission(route: ActivatedRoute) {
    const functionCode = route.snapshot.data.functionCode;
    this.functions = JSON.parse(localStorage.getItem('functions')).filter(x => x.functionCode === functionCode) || [];
    for (const item of this.functions) {
        const toolbarOptions = [];
        for (const action of item.childrens) {
          const optionItem = this.makeAction(action.code);
          toolbarOptions.push(...optionItem.filter(Boolean));
        }
        toolbarOptions.push('Search');
        const uniqueOptionItem = toolbarOptions.filter((elem, index, self) => {
          return index === self.indexOf(elem);
        });
        this.toolbarOptions = uniqueOptionItem;
    }
  }
  protected PermissionForTreeGrid(route: ActivatedRoute) {
    this.contextMenuItems = [];
    this.functions = JSON.parse(localStorage.getItem('functions'));
    for (const item of this.functions) {
      if (route.snapshot.data.functionCode.includes(item.functionCode)) {
        const toolbarOptionsTree = [];
        for (const action of item.childrens) {
          const optionItem = this.makeActionTreeGrid(action.code);
          toolbarOptionsTree.push(...optionItem.filter(Boolean));
        }
        toolbarOptionsTree.push(...['Search',
          'ExpandAll',
          'CollapseAll',
          'ExcelExport']);
        const uniqueOptionItem = toolbarOptionsTree.filter((elem, index, self) => {
          return index === self.indexOf(elem);
        });
        this.toolbarOptionsTree = uniqueOptionItem;
        break;
      }
    }
  }
  // Đổi action code thanh action của ej2-grid
  protected makeAction(input: string): string[] {
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

  protected makeActionTreeGrid(input: string): string[] {
    switch (input) {
      case ActionConstant.EXCEL_EXPORT:
        return ['ExcelExport'];
      case ActionConstant.CREATE:
        this.editSettingsTree.allowAdding = true;
        this.contextMenuItems.push({
          text: 'Add Child',
          iconCss: ' e-icons e-add',
          target: '.e-content',
          id: 'Add-Sub-Item'
        });
        return ['Add'];
      case ActionConstant.EDIT:
        this.editSettingsTree.allowEditing = false;
        return [undefined];
      case ActionConstant.DELETE:
        this.editSettingsTree.allowDeleting = false;
        this.contextMenuItems.push({
          text: 'Delete',
          iconCss: ' e-icons e-delete',
          target: '.e-content',
          id: 'DeleteOC'
        });
        return [undefined];
      default:
        return [undefined];
    }
  }
  constructor(
    public translate: TranslateService,
    ) {
      // console.log('load truoc ngOnInit');
    let user = JSON.parse(localStorage.getItem('user'));
    let pageSize = Number(user?.pageSizeSettingValue) || 20;
    let pageSizesTemp = user?.pageSizeSettingList || ['5', '10', '12', '20'];
    let pageSizes = pageSizesTemp.map(x=> +x);
    this.pageSettings = {  pageSizes: pageSizes, enableQueryString: true,  pageSize: pageSize, currentPage: 1, enableScroll: true };
    this.take = this.pageSettings.pageSize;

    let pageSettingColor = JSON.parse(localStorage.getItem('colorPageSetting'));
    this.pageSettingsColor = {
      pageCount: pageSettingColor.pageCount,
      pageSizes: pageSettingColor.pageSizes,
      pageSize: pageSettingColor.pageSize,
    }
    // console.log(this.pageSettingsColor);
  }
  convertDate(data) {
    if ( data instanceof Date ) {
      return (data as Date).toLocaleDateString();
    }
    return data;
  }
}
