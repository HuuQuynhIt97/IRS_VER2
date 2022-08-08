import { TreatmentWayService } from './../../../../_core/_service/setting/treatment-way.service';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TreatmentWay } from 'src/app/_core/_model/setting/treatmentWay';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { ProcessService } from 'src/app/_core/_service/setting/process.service';

@Component({
  selector: 'app-treatment-way',
  templateUrl: './treatment-way.component.html',
  styleUrls: ['./treatment-way.component.scss']
})
export class TreatmentWayComponent extends BaseComponent implements OnInit, OnDestroy {
  public toolbarOptions = [ 'Add', 'Cancel', 'Search'];
  public editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  public data: object[];
  filterSettings = { type: 'Excel' };
  modelTreatmentWay: TreatmentWay  = {} as TreatmentWay
  public ProcessData: any = [];
  public textProcess = 'Select Treament';
  public fieldsSup: object = { text: 'name', value: 'name' };
  @ViewChild('grid') grid: GridComponent;
  public textGlueLineName = 'Select ';
  public treatmentWay: TreatmentWay[];
  public setFocus: any;
  nameDefault: any;
  processDefault: any;
  nameEnDefault: any;
  constructor(
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private service: TreatmentWayService,
    private serviceProcess: ProcessService,
    public translate: TranslateService,
  ) { 
    super(translate);
  }
  ngOnDestroy(): void {
    // throw new Error('Method not implemented.');
  }

  ngOnInit(): void {
    this.getAll();
    this.getAllProcess();
  }

  onChangeTreatment(args) {
    this.modelTreatmentWay.process = args.value;
  }

  getAll() {
    this.service.getAll().subscribe((res) => {
      console.log(res);
      this.treatmentWay = res;
    });
  }
  getAllProcess() {
    this.serviceProcess.getAll().subscribe((res: any) => {
      this.ProcessData = res;
    });
  }

  actionBegin(args) {
    let nameNew = null;
    let nameEnNew = null;
    if (args.requestType === "beginEdit" ) {
      this.nameDefault = args.rowData.name ;
      this.nameEnDefault = args.rowData.nameEn ;
      this.processDefault = args.rowData.process;
      this.modelTreatmentWay.id = args.rowData.id || 0;
      this.modelTreatmentWay.name = args.rowData.name;
      this.modelTreatmentWay.nameEn = args.rowData.nameEn;
      this.modelTreatmentWay.process = args.rowData.process;
    }
    if (args.requestType === 'save') {
      if (args.action === 'edit') {
        this.modelTreatmentWay.id = args.data.id || 0 ;
        this.modelTreatmentWay.name = args.data.name ;
        this.modelTreatmentWay.nameEn = args.data.nameEn ;
        nameNew = args.data.name ;
        nameNew = args.data.nameEn ;
        if (this.nameDefault !== nameNew || this.nameEnDefault !== nameEnNew || this.processDefault !== this.modelTreatmentWay.process) {
          this.update(this.modelTreatmentWay) ;
        } else {
          this.grid.refresh() ;
        }
      }
      if (args.action === 'add') {
        const dataSource = this.grid.dataSource as any
        const exist = dataSource.filter(x => x.name === args.data.name && x.process === this.modelTreatmentWay.process)
        if (this.modelTreatmentWay.process === undefined || this.modelTreatmentWay.process === null) {
          this.alertify.error("Please select Treatment");
          args.cancel = true;
          return ;
        }
        if (exist.length > 0) {
          this.alertify.error("Data already exists");
          args.cancel = true;
          return ;
        }
        this.modelTreatmentWay.id = 0 ;
        this.modelTreatmentWay.name = args.data.name ;
        if (args.data.name !== undefined && this.modelTreatmentWay.process !== null) {
          this.add(this.modelTreatmentWay) ;
        } else {
          this.getAll() ;
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
      (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
    }
    if (e.requestType === 'beginEdit') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
      (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
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
              this.getAll();
            } else {
              this.alertify.warning(this.alert.system_error_msg);
            }
          },
          (err) => this.alertify.warning(this.alert.system_error_msg)
        );
      }, () => {
        this.alertify.error(this.alert.cancelMessage);
        this.getAll();
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
        this.service.update(modalSup as TreatmentWay).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.getAll();
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
        this.getAll();
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
              this.modelTreatmentWay.name = '';
              this.modelTreatmentWay.nameEn = '';
              this.getAll();
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
        this.getAll();
      }
    );
   
  }

  

  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }

}
