import { WorkPlanService } from './../../../../_core/_service/execution/work-plan.service';
import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CalendarView } from '@syncfusion/ej2-angular-calendars';
import { Column, GridComponent } from '@syncfusion/ej2-angular-grids';
import { Tooltip } from '@syncfusion/ej2-angular-popups';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { environment } from 'src/environments/environment';
import { MessageConstants } from 'src/app/_core/_constants';

@Component({
  selector: 'app-workplan',
  templateUrl: './workplan.component.html',
  styleUrls: ['./workplan.component.scss']
})
export class WorkplanComponent implements OnInit {

  @ViewChild('grid')
  public gridObj: GridComponent;
  modalReference: NgbModalRef;
  modalAddReference: NgbModalRef;
  @ViewChild('importModal', { static: true })
  importModal: TemplateRef<any>;

  @ViewChild('content', { static: true })
  content: TemplateRef<any>;
  total: any;
  added: any;
  dataExist: any;
  noAdd: any;
  failedAdd: any;
  public start: CalendarView = 'Year';
  public depth: CalendarView = 'Year';
  public format: string = 'MMMM y'
  workPlanDate_system: string;
  time_upload: any;
  locale: string = localStorage.getItem('lang')
  public dateValue: any = new Date();
  workPlanDate: any = new Date();
  systemDate: Date;
  file: any;
  name: string;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 20 };
  data: any[];
  editSettings: object;
  filterSettings: { type: string; };
  excelDownloadUrl: string;
  apiUrl = environment.apiUrl.replace('/api', '') + 'images/workplan-format.png';
  toolbar: object;
  constructor(
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private datePipe: DatePipe,
    private router: Router,
    private service: WorkPlanService
  ) { }

  ngOnInit() {
    this.excelDownloadUrl = `${environment.apiUrl}workplan/ExcelExport`;
    this.toolbar = [ 'Search'];
    this.filterSettings = { type: 'Excel' };
    this.editSettings = { allowEditing: false, allowAdding: false, allowDeleting: false, newRowPosition: 'Normal' };
    this.getAll();
  }

  export() {
    this.service.export(this.failedAdd).subscribe((data: any) => {
      const blob = new Blob([data],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
      const downloadURL = window.URL.createObjectURL(data);
      const link = document.createElement('a');
      link.href = downloadURL;
      link.download = `WorkPlan.xlsx`;
      link.click();
    });

  }
  onChangeDate(args) {
    this.workPlanDate = this.datePipe.transform(args.value,'yyyy-MM-dd')

  }
  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }
  uploadFile() {
    if (this.file === undefined || this.file.length === 0){
      this.alertify.error('No data upload')
      return;
    }
    this.service.importWorkPlanNew(this.file, this.datePipe.transform(this.workPlanDate, 'yyyy-MM-dd'))
    .subscribe((res: any) => {
      // if(res.status) {
      //   this.modalReference.close();
      //   this.getAll();
      //   this.total = res.total
      //   this.added = res.added
      //   this.dataExist = res.dataExist
      //   this.noAdd = res.noAdd
      //   this.failedAdd = res.failedAdd
      //   this.openVerticallyCentered(this.content) ;
      // } else {
      //   this.alertify.error('The uploaded file has a date format error, please check the format and re-upload')
      // }
      this.getAll();
      this.modalReference.close();
      this.alertify.success(MessageConstants.CREATED_OK_MSG);
    });

  }
  fileProgress(event) {
    this.file = event.target.files[0];
  }
  recordDoubleClick(args) {
    // const url = this.router.serializeUrl(this.router.createUrlTree([`ink/establish/schedule/detail-workplan/${args.id}/${args.treatment}`]))
    // window.open(url,'_blank')

    // this.GetDetailSchedule(args.rowData.id);
    // this.modalReference = this.modalService.open(editModal, { size: 'xxl' });
  }
  NO(index) {
    return (this.gridObj.pageSettings.currentPage - 1) * this.gridObj.pageSettings.pageSize + Number(index) + 1;
  }
  excelExportComplete(): void {
    (this.gridObj.columns[0] as Column).visible = false;
    (this.gridObj.columns[11] as Column).visible = false;
  }
  tooltip(args) {
    if (args.column.field === 'modelName' && args.data.status === false) {
      const tooltip: Tooltip = new Tooltip({
          content: 'Recipe No Data'
      }, args.cell as HTMLTableCellElement);
    }
  }

  search(args) {
    this.gridObj.search(this.name)
  }

  showModal(importModal) {
    this.modalReference = this.modalService.open(importModal, { size: 'xl' });
    this.modalReference.result.then((result) => {
      this.file = []
    }, (reason) => {
      this.file = []
    });
  }
  toolbarClick(args) {
    const target: HTMLElement = (args.target as HTMLElement).closest('button');
    switch (target?.id) {
      case 'Import Excel':
        this.showModal(this.importModal) ;
        break;
      case 'Export Excel':
        // const data = this.data.map(item => {
        //   return {
        //     approvalStatus: item.approvalStatus === true ? 'Yes' : 'No',
        //     articleNo: item.articleNo,
        //     establishDate: this.datePipe.transform(item.establishDate, 'yyyy-MM-dd'),
        //     productionDate: this.datePipe.transform(item.productionDate, 'yyyy-MM-dd'),
        //     artProcess: item.artProcess,
        //     finishedStatus: item.finishedStatus === true ? 'Yes' : 'No',
        //     modelNo: item.modelNo,
        //     modelName: item.modelName,
        //     treatment: item.treatment,
        //     process: item.process,
        //     parts: item.parts.join(' - ') || '#N/A',
        //   };
        // });
        (this.gridObj.columns[0] as Column).visible = false;
        (this.gridObj.columns[11] as Column).visible = false;
        const exportProperties = {
          // dataSource: data,
          fileName: 'ScheduleData.xlsx'
        };
        this.gridObj.excelExport(exportProperties);
        break;
    }
  }

  onChangeSystemDate(args) {
    this.systemDate = args.value as Date
    this.workPlanDate_system = this.datePipe.transform(args.value,'yyyy-MM')
    if (this.systemDate !== null)
      this.getAllByDate();
  }

  getAllByDate() {
    // this.scheduleService.getAllWorkPlanWithDate(this.systemDate.toDateString()).subscribe( (res: any) => {
    //   this.data = res.result ;
    //   this.time_upload = res.time_upload ;
    //   // this.data = res.map(item => {
    //   //   return {
    //   //     id: item.id,
    //   //     approvalStatus: item.approvalStatus === true ? 'Yes' : 'No',
    //   //     articleNo: item.articleNo,
    //   //     establishDate: this.datePipe.transform(item.establishDate, 'yyyy-MM-dd'),
    //   //     productionDate: this.datePipe.transform(item.productionDate, 'yyyy-MM-dd'),
    //   //     artProcess: item.artProcess,
    //   //     finishedStatus: item.finishedStatus === true ? 'Yes' : 'No',
    //   //     modelNo: item.modelNo,
    //   //     modelName: item.modelName,
    //   //     treatment: item.treatment,
    //   //     process: item.process,
    //   //     parts: item.parts,
    //   //     approvalBy: item.approvalBy,
    //   //     createdBy: item.createdBy,
    //   //     processID: item.processID,
    //   //   };
    //   // });
    // });
  }

  onClickDefault() {
    this.systemDate = null
    this.workPlanDate_system = null
    this.getAll()
  }

  getAll() {
    // this.service.getAllWorkPlan().subscribe( (res: any) => {
    //   this.data = res.result ;
    //   this.time_upload = res.time_upload ;
    //   // this.data = res.map(item => {
    //   //   return {
    //   //     id: item.id,
    //   //     approvalStatus: item.approvalStatus === true ? 'Yes' : 'No',
    //   //     articleNo: item.articleNo,
    //   //     establishDate: this.datePipe.transform(item.establishDate, 'yyyy-MM-dd'),
    //   //     productionDate: this.datePipe.transform(item.productionDate, 'yyyy-MM-dd'),
    //   //     artProcess: item.artProcess,
    //   //     finishedStatus: item.finishedStatus === true ? 'Yes' : 'No',
    //   //     modelNo: item.modelNo,
    //   //     modelName: item.modelName,
    //   //     treatment: item.treatment,
    //   //     process: item.process,
    //   //     parts: item.parts,
    //   //     approvalBy: item.approvalBy,
    //   //     createdBy: item.createdBy,
    //   //     processID: item.processID,
    //   //   };
    //   // });
    // });

    this.service.getAllWorkPlanNew().subscribe((res: any) => {
      this.data = res.result;
      this.time_upload = res.time_upload;
    });
  }

}
