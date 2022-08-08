import { ProcessService } from './../../../../_core/_service/setting/process.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Process } from 'src/app/_core/_model/setting/process';

@Component({
  selector: 'app-treatment',
  templateUrl: './treatment.component.html',
  styleUrls: ['./treatment.component.scss']
})
export class TreatmentComponent extends BaseComponent implements OnInit, OnDestroy {
  public toolbarOptions = [ 'Add', 'Cancel', 'Search'];
  public editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  public data: object[];
  filterSettings = { type: 'Excel' };
  
  public ProcessData: any = [];
  public textProcess = 'Select Treament';
  public fieldsSup: object = { text: 'name', value: 'name' };
  @ViewChild('grid') grid: GridComponent;
  public textGlueLineName = 'Select ';
  public treatmentWay: object[];
  public setFocus: any;
  nameDefault: any;
  processDefault: any;
  color: string = '#17a2b8'
  modelProcess: Process  = {} as Process;
  colorDefault: any;
  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: ProcessService,
    public translate: TranslateService,
  ) {
    super(translate);
  }
  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnInit(): void {
    this.getAllProcess();
  }

  getAllProcess() {
    this.service.getAll().subscribe((res: any) => {
      this.ProcessData = res;
    });
  }

  actionBegin(args) {
    let nameNew = null;
    let coloNew = null;
    if (args.requestType === "beginEdit" ) {
      this.nameDefault = args.rowData.name ;
      this.colorDefault = args.rowData.color ;
      this.color = args.rowData.color;
      this.processDefault = args.rowData.process;
      this.modelProcess.id = args.rowData.id || 0;
      this.modelProcess.name = args.rowData.name;
      this.modelProcess.color = args.rowData.color;
    }
    if (args.requestType === 'save') {
      if (args.action === 'edit') {
        this.modelProcess.id = args.data.id || 0 ;
        this.modelProcess.name = args.data.name ;
        this.modelProcess.color = this.color ;
        const dataSource = this.grid.dataSource as any
        // const exist = dataSource.filter(x => x.name === args.data.name && args.data.color === this.color)
        // if (exist.length > 0) {
        //   this.alertify.error("Data already exists");
        //   args.cancel = true;
        //   return ;
        // }
         nameNew = args.data.name ;
         coloNew = this.color;
        if (this.nameDefault !== nameNew || this.colorDefault !== coloNew  ) {
          this.update(this.modelProcess) ;
        } else {
          this.grid.refresh() ;
        }
      }
      if (args.action === 'add') {
        const dataSource = this.grid.dataSource as any
        const exist = dataSource.filter(x => x.name === args.data.name)
        if (args.data.name === undefined ) {
          this.alertify.error("Please Input Treatment");
          args.cancel = true;
          return ;
        }
        if (exist.length > 0) {
          this.alertify.error("Data already exists");
          args.cancel = true;
          return ;
        }
        this.modelProcess.id = 0 ;
        this.modelProcess.name = args.data.name ;
        this.modelProcess.color = this.color;
        if (args.data.name !== undefined) {
          this.add(this.modelProcess) ;
        } else {
          this.getAllProcess() ;
          this.grid.refresh() ;
        }
      }
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id) ;
    }
  }

  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      default:
        break;
    }
  }

  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
    }
    if (e.requestType === 'beginEdit') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
    }
  }

  onDoubleClick(args: any): void {
    this.setFocus = args.column;  // Get the column from Double click event
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
              this.getAllProcess();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (err) => this.alertify.warning(this.alert.system_error_msg)
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getAllProcess();
      }
    );

  }

  update(modalSup) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.service.update(modalSup as Process).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.getAllProcess();
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
        this.getAllProcess();
      }
    );
  
  }

  add(modalSup) {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.createTitle,
      this.alert.createMessage,
      () => {
        this.service.add(modalSup).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.modelProcess.name = '';
              this.color = '#17a2b8'
              this.getAllProcess();
              // this.modalReference.dismiss();

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
        this.getAllProcess();
      }
    );
   
  }

 

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }

}
