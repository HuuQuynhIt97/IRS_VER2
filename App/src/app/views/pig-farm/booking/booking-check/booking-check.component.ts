import { BookingService } from './../../../../_core/_service/booking/booking.service';
import { WorkOrder } from './../../../../_core/_model/booking/workOrder';
import { WorkorderService } from './../../../../_core/_service/booking/workorder.service';
import { SiteScreen } from './../../../../_core/_model/club-management/site';
import { Hall } from './../../../../_core/_model/club-management/hall';
import { HallService } from './../../../../_core/_service/club-management/hall.service';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { L10n,setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, OnDestroy, Input } from '@angular/core';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { BookingDetail } from 'src/app/_core/_model/booking/bookingDetail';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { RoomService } from 'src/app/_core/_service/farms';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { DatePickerComponent } from '@syncfusion/ej2-angular-calendars';

@Component({
  selector: 'app-booking-check',
  templateUrl: './booking-check.component.html',
  styleUrls: ['./booking-check.component.scss']
})
export class BookingCheckComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang =  (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectArea = new EventEmitter();
  data: any;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick'};
  subscription: Subscription;
  site: SiteScreen;
  rowIndex: any;
  hallData: any;
  hallFields: object = { text: 'hallName', value: 'guid' };

  roomData: any;
  roomFields: object = { text: 'roomName', value: 'guid' };

  workOrderData: any;
  workOrderFields: object = { text: 'workOrderName', value: 'guid' };

  xAccountData: any;
  xAccountFields: object = { text: 'accountName', value: 'guid' };
  public dateValue: null;
  model: BookingDetail;
  siteId: string = localStorage.getItem('farmGuid');
  start_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),0,0);
  end_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),23,59);
  dataFilter: Object;
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  @ViewChild('roomDropdown') public roomDropdown: DropDownListComponent;
  @ViewChild('funeralDropdown') public funeralDropdown: DropDownListComponent;
  @ViewChild('date') public date: DatePickerComponent;
  fiterRequest = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: null
  }

  fiterRequestDefault = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: null
  }
  noGuid: string = '';
  constructor(
    private service: BookingService,
    private serviceHall: HallService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private serviceXaccount: XAccountService,
    private serviceRoom: RoomService,
    private serviceWorkOrder: WorkorderService,
    public translate: TranslateService,
  ) { super(translate);  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  change(args: any) {
    this.pageSettings.currentPage=args.value;
}
  ngOnInit() {
    this.siteId = localStorage.getItem('farmGuid') ;
    this.loadData();
  }
  changeTime(args) {
    this.fiterRequest.bookingDate = this.datePipe.transform(args.value , 'yyyy-MM-dd') 
    this.search();
  }
  refresh(){
    this.hallDropdown.value = null
    this.roomDropdown.value = null
    this.funeralDropdown.value = null
    this.fiterRequest.bookingDate = null
    this.date.value = null
    this.searchDefault()
  }
  loadData() {
    this.service.searchBookingCheck(this.fiterRequest).subscribe((res: any) => {
      this.data = res
      if(res?.length > 0) {
        this.loadHallData()
        this.loadFuneralDirector()
      }
    })
  }
  search() {
    return new Promise((res, rej) => {
      this.service.searchBookingCheck(this.fiterRequest).subscribe(
        (result) => {
          this.data = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  searchDefault() {
    return new Promise((res, rej) => {
      this.service.searchBookingCheck(this.fiterRequestDefault).subscribe(
        (result) => {
          this.data = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }
  loadFuneralDirector() {
    this.serviceXaccount.getXAccountsBySite(this.siteId).subscribe(res => {
      this.xAccountData = res
      this.xAccountData.unshift({ 
        id: 0, 
        guid: '', 
        accountName: this.translate.instant('DESKTOP_NO_ITEM_DATA') });
      this.noGuid = 'noGuid'
    })
  }
  async ngModelChangeHallFilter(args) {
    this.fiterRequest.hallGuidFiter = args.value
    await this.search();
    await this.loadRoomData(args.value)
  }
  async ngModelChangeRoomFilter(args) {
    this.fiterRequest.roomGuidFiter = args.value
    await this.search();
  }
  async ngModelChangefuneralFilter(args) {
    this.fiterRequest.funeralDirectorFilter = args.value
    await this.search();
  }
  async openModal(template, data = {} as BookingDetail) {
    this.loadHallData()
    this.loadFuneralDirector()
     if (data?.id > 0) {
      const item = await this.getById(data.id);
      this.model = {...item};
      await this.loadRoomData(this.model.hallGuid)
      await this.loadWorkOrderData(this.model.hallGuid)
      this.getAudit(this.model.id);
      this.title = 'BOOKING_CHECK_MODAL';
     } else {
       this.model.id = 0;
       this.title = 'BOOKING_CHECK_MODAL';
     }
     this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
  }

  loadRoomData(hallID) {
    return new Promise((res, rej) => {
      this.serviceRoom.getRoomBySiteAndHall(this.siteId,hallID).subscribe(
        (result) => {
          this.roomData = result
          this.roomData.unshift({ 
            id: 0, 
            guid: '', 
            roomName: this.translate.instant('DESKTOP_NO_ITEM_DATA') });
          this.noGuid = 'noGuid'
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadWorkOrderData(hallID) {
    return new Promise((res, rej) => {
      this.serviceWorkOrder.GetByHall(hallID).subscribe(
        (result) => {
          this.workOrderData = result
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }


  loadHallData() {
    return new Promise((res, rej) => {
      this.serviceHall.loadDataBySiteGuiId(this.siteId).subscribe(
        (result) => {
          this.hallData = result
          this.hallData.unshift({ 
            id: 0, 
            guid: '', 
            hallName: this.translate.instant('DESKTOP_NO_ITEM_DATA') });
          this.noGuid = 'noGuid'
        },
        (error) => {
          rej(error);
        }
      );
    });
  }
  ngModelChange(value) {
    this.model.hallGuid = value || "";
  }
  typeChange(value) {
    // this.model.type = value || "";
  }
  // life cycle ejs-grid
  rowSelected(args) {
    this.rowIndex = args.rowIndex;

  }
 

  toolbarClick(args) {
    switch (args.item.id) {
      case 'grid_excelexport':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
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


  // end life cycle ejs-grid

  // api
  getAudit(id) {
    this.service.getAudit(id).subscribe(data => {
      this.audit = data;
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
  public onToolbarItemClicked(args){
    switch (args.item.id) {
      case 'grid_add':
        args.cancel = true;
        this.model = {} as any;
        this.openModal(this.templateRef);
        break;
      default:
        break;
    }
  }
  getById(id) {
   return this.service.getById(id).toPromise();
  }
  create() {
    this.alertify.confirm4(
       this.alert.yes_message,
       this.alert.no_message,
       this.alert.createTitle,
       this.alert.createMessage,
       () => {
         this.model.siteGuid = this.siteId
         this.model.bookingTimeS = this.datePipe.transform(this.start_time, "HH:mm")
         this.model.startDate = this.datePipe.transform(this.start_time, "yyyy/MM/dd HH:mm")
         this.model.bookingTimeE = this.datePipe.transform(this.end_time, "HH:mm")
         this.model.endDate = this.datePipe.transform(this.end_time, "yyyy/MM/dd HH:mm")
         this.model.orderDate = this.datePipe.transform(this.dateValue, "yyyy/MM/dd")
         this.model.bookingDate = this.datePipe.transform(this.dateValue, "yyyy/MM/dd")
         this.service.add(this.model).subscribe(
           (res) => {
            if(res.message === 'Exist') {
              this.alertify.error(this.translate.instant('BOOKING_EXIST'))
              // this.alertify.warning(this.alert.system_error_msg);
            }else {
              this.alertify.success(this.alert.created_ok_msg);
               this.modalReference.dismiss();
               this.loadData()
              // this.alertify.error('Add Failed')
            }
            //  if (res.success === true) {
               
 
            //  } else {
            //    this.alertify.warning(this.alert.system_error_msg);
            //  }
 
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
   async ngModelChangeHall(value) {
    await this.loadRoomData(value)
    await this.loadWorkOrderData(value)
    // this.model.hallGuid = value || "";
  }
   ToDate(date: any) {
     if (date &&  date instanceof Date) {
       return this.datePipe.transform(date, "yyyy/MM/dd");
     } else if (date && !(date instanceof Date)) {
       return date;
     }
      else {
       return null;
     }
   }
   update() {
    this.alertify.confirm4(
       this.alert.yes_message,
       this.alert.no_message,
       this.alert.updateTitle,
       this.alert.updateMessage,
       () => {
         this.service.update(this.model as BookingDetail).subscribe(
           (res) => {
             if (res.success === true) {
               this.alertify.success(this.alert.updated_ok_msg);
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
}
