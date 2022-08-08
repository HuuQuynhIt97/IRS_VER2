import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from "@angular/router";
import { ActionConstant, MessageConstants } from "../_constants";
import { FunctionSystem } from "../_model/application-user";
import { environment } from 'src/environments/environment';

export abstract class BaseDetailComponent {
  globalLang = localStorage.getItem('lang');
  skip = 0;
  take = 10;
  functionName: string;
  printBy: string;
  audit: any;
  alert = {
    updateMessage: MessageConstants.UPDATE_MESSAGE,
    updateTitle: MessageConstants.UPDATE_TITLE,
    createMessage:MessageConstants.CREATE_MESSAGE,
    createTitle: MessageConstants.CREATE_TITLE,
    deleteMessage: MessageConstants.DELETE_MESSAGE,
    deleteTitle: MessageConstants.DELETE_TITLE,
    cancelMessage: MessageConstants.CANCEL_MESSAGE,
    serverError: MessageConstants.SERVER_ERROR,
    deleted_ok_msg: MessageConstants.DELETED_OK_MSG,
    created_ok_msg: MessageConstants.CREATED_OK_MSG,
    updated_ok_msg: MessageConstants.UPDATED_OK_MSG,
    system_error_msg: MessageConstants.SYSTEM_ERROR_MSG,
    exist_message: MessageConstants.EXIST_MESSAGE,
    choose_farm_message: MessageConstants.CHOOSE_FARM_MESSAGE,
    select_order_message: MessageConstants.SELECT_ORDER_MESSAGE,
    yes_message: MessageConstants.YES_MSG,
    no_message: MessageConstants.NO_MSG,
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
  constructor(public translate: TranslateService) {
    this.getAlertTranslator();
  }
  getAlertTranslator() {
    this.translate.get(this.alert.updateMessage).subscribe((res: string) => {
      if (res) {
        this.alert.updateMessage = res;
      }
    });
    this.translate.get(this.alert.updateTitle).subscribe((res: string) => {
      if (res) {
        this.alert.updateTitle = res;
      }
    });
    this.translate.get(this.alert.createMessage).subscribe((res: string) => {
      if (res) {
        this.alert.createMessage = res;
      }
    });
    this.translate.get(this.alert.createTitle).subscribe((res: string) => {
      if (res) {
        this.alert.createTitle = res;
      }
    });
    this.translate.get(this.alert.deleteMessage).subscribe((res: string) => {
      if (res) {
        this.alert.deleteMessage = res;
      }
    });
    this.translate.get(this.alert.deleteTitle).subscribe((res: string) => {
      if (res) {
        this.alert.deleteTitle = res;
      }
    });

    this.translate.get(this.alert.serverError).subscribe((res: string) => {
      if (res) {
        this.alert.serverError = res;
      }
    });
    this.translate.get(this.alert.cancelMessage).subscribe((res: string) => {
      if (res) {
        this.alert.cancelMessage = res;
      }
    });
    this.translate.get(this.alert.choose_farm_message).subscribe((res: string) => {
      if (res) {
        this.alert.choose_farm_message = res;
      }
    });

    this.translate.get(this.validationGrid.requireField).subscribe((res: string) => {
      if (res) {
        this.validationGrid.requireField = res;
      }
    });
    this.translate.get(this.validationGrid.textLength).subscribe((res: string) => {
      if (res) {
        this.validationGrid.textLength = res;
      }
    });
  }
}
