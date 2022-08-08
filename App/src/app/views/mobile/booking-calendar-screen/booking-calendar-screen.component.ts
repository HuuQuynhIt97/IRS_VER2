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
import { WorkorderService } from 'src/app/_core/_service/booking/workorder.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';
import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { SiteScreen } from 'src/app/_core/_model/club-management/site';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { Router } from '@angular/router';
import { LanguageConstant } from 'src/app/_core/_constants/language.constant';

@Component({
  selector: 'app-booking-calendar-screen',
  templateUrl: './booking-calendar-screen.component.html',
  styleUrls: ['./booking-calendar-screen.component.scss']
})
export class BookingCalendarScreenComponent extends BaseComponent implements OnInit, OnDestroy  {

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
  public dateValue: Date = new Date();
  model: BookingDetail;
  siteId: string = localStorage.getItem('farmGuid');
  start_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),0,0);
  end_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),23,59);
  dataFilter: Object;
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  @ViewChild('siteDropdown') public siteDropdown: DropDownListComponent;
  @ViewChild('funeralDropdown') public funeralDropdown: DropDownListComponent;
  @ViewChild('date') public date: DatePickerComponent;
  fiterRequest = {
    siteGuidFilter: '',
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: this.datePipe.transform(this.dateValue , 'yyyy-MM-dd') 
  }
  fiterRequestDefault = {
    siteGuidFilter: '',
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: this.datePipe.transform(this.dateValue , 'yyyy-MM-dd') 
  }
  noGuid: string = ''
  siteData: any;
  siteFields: object = { text: 'siteName', value: 'guid' };
  dateLocale: string = 'en-US'
  constructor(
    private service: BookingService,
    private serviceHall: HallService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private serviceXaccount: XAccountService,
    private serviceRoom: RoomService,
    private serviceSite: SiteService,
    private serviceWorkOrder: WorkorderService,
    public translate: TranslateService,
    public router: Router,
  ) { super(translate);  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  change(args: any) {
    this.pageSettings.currentPage=args.value;
}
  ngOnInit() {
    // this.Permission(this.route);
    let languages = JSON.parse(localStorage.getItem('languages'));
    this.siteId = localStorage.getItem('farmGuid') ;
    let lang = localStorage.getItem('lang');
    if(lang === LanguageConstant.EN) {
      this.dateLocale = 'en-US'
    }
    if(lang === LanguageConstant.VI) {
      this.dateLocale = 'vi'
    }
    if(lang === LanguageConstant.TW || lang === LanguageConstant.CN) {
      this.dateLocale = 'zh'
    }
    // setCulture(lang);
    // let load = {
    //   [lang]: {
    //     grid: languages['grid'],
    //     pager: languages['pager']
    //   }
    // };
    // L10n.load(load);
    this.loadSiteData()
    // this.loadData();
    // this.loadFuneralDirector()
  }
  changeTime(args) {
    this.fiterRequest.bookingDate = this.datePipe.transform(args.value , 'yyyy-MM-dd') 
    this.search()
  }

  async ngModelChangeSiteFilter(args) {
    if(args.isInteracted) {
      this.fiterRequest.siteGuidFilter = args.value
      await this.search()
      await this.loadHallData(args.value)
    }
  }
  async ngModelChangeHallFilter(args) {
    if(args.isInteracted) {
      this.fiterRequest.hallGuidFiter = args.value
      await this.search()
    }
  }
  refresh(){
    this.siteDropdown.value = null
    this.hallDropdown.value = null
    this.fiterRequest.bookingDate = this.datePipe.transform(new Date() , 'yyyy-MM-dd')
    this.date.value = new Date()
    this.searchDefault()
  }
  detailRoom(roomID,bookingID) {
    this.router.navigate([`/mobile/hall-detail/${roomID}/${bookingID}`]);
  }
  searchDefault() {
    return new Promise((res, rej) => {
      this.service.getSearchBookingCalendar(this.fiterRequestDefault).subscribe(
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
  search() {
    return new Promise((res, rej) => {
      this.service.getSearchBookingCalendar(this.fiterRequest).subscribe(
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
  loadSiteData() {
    return new Promise((res, rej) => {
      this.serviceSite.getAll().subscribe(
        (result) => {
          this.siteData = result
          this.siteData.unshift({ 
            id: 0, 
            guid: 'noGuid', 
            siteName: this.translate.instant('MOBILE_NO_ITEM_DATA') });
            this.noGuid = 'noGuid'
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
    })
  }
  
  async openModal(template, data = {} as BookingDetail) {
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
  loadData() {
    this.service.getBookingCheck().subscribe(res => {
      this.data = res
    })
    // this.data = new DataManager({
    //   url: `${this.baseUrl}WorkOrder/LoadData`,
    //   adaptor: new UrlAdaptor,
    // });
  }
  loadHallData(siteID) {
    return new Promise((res, rej) => {
      this.serviceHall.loadDataBySiteGuiId(siteID).subscribe(
        (result) => {
          this.hallData = result
          this.hallData.unshift({ 
            id: 0, 
            guid: 'noGuid', 
            hallName: this.translate.instant('MOBILE_NO_ITEM_DATA') });
          this.noGuid = 'noGuid'
          res(result);
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
