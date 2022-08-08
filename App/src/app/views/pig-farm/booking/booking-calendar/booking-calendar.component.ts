import { L10n, loadCldr } from '@syncfusion/ej2-base';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { BookingService } from './../../../../_core/_service/booking/booking.service';
import { BookingDetail } from './../../../../_core/_model/booking/bookingDetail';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { WorkorderService } from './../../../../_core/_service/booking/workorder.service';
import { RoomService } from 'src/app/_core/_service/farms';
import { Component, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ClickEventArgs } from '@syncfusion/ej2-angular-buttons';
import { EventRenderedArgs, EventSettingsModel, PopupOpenEventArgs, ScheduleComponent, View } from '@syncfusion/ej2-angular-schedule';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Hall } from 'src/app/_core/_model/club-management/hall';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';
import { DatePipe } from '@angular/common';
import { environment } from 'src/environments/environment';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { LanguageConstant } from 'src/app/_core/_constants/language.constant';

@Component({
  selector: 'app-booking-calendar',
  templateUrl: './booking-calendar.component.html',
  styleUrls: ['./booking-calendar.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BookingCalendarComponent extends BaseComponent implements OnInit {
  public dataSchedular: Record<string, any>[]
  selectedDate: Date = new Date(new Date().getFullYear(), new Date().getUTCMonth(),new Date().getDate());
  funeralGuildSystem = JSON.parse(localStorage.getItem('user'))?.guid
  currentView: View = 'Month';
  enableAdaptiveUI: false;
  model: BookingDetail = {} as BookingDetail;
  baseUrl = environment.apiUrl;
  title: any;
  audit: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  modalReference: NgbModalRef;
  siteId: string = localStorage.getItem('farmGuid');
  hallData: any;
  hallFields: object = { text: 'hallName', value: 'guid' };

  roomData: any;
  roomFields: object = { text: 'roomName', value: 'guid' };

  workOrderData: any;
  workOrderFields: object = { text: 'workOrderName', value: 'guid' };

  xAccountData: any;
  xAccountFields: object = { text: 'accountName', value: 'guid' };

  start_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),0,0);
  start_time_default = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),0,0);
  end_time = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),23,59);
  end_time_default = new Date(new Date().getFullYear(), new Date().getUTCMonth(), new Date().getDate(),23,59);

  public dateValue: Date = new Date();
  eventSettings: EventSettingsModel = { dataSource: null };
  @ViewChild("scheduleObj")
  public scheduleObj: ScheduleComponent;
  public eventAdded: boolean = false;
  fiterRequest: any = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: ''
  }
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  @ViewChild('roomDropdown') public roomDropdown: DropDownListComponent;
  @ViewChild('funeralDropdown') public funeralDropdown: DropDownListComponent;
  localeSchedule: string = 'en-US'
  constructor(
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private service: BookingService,
    private serviceHall: HallService,
    private datePipe: DatePipe,
    private serviceRoom: RoomService,
    private serviceWorkOrder: WorkorderService,
    private serviceXaccount: XAccountService,
    public translate: TranslateService
  ) {super(translate); }

  ngOnInit() {
    this.getPreviewScheduler()
    this.loadHallData()
    this.loadFuneralDirector()
    this.siteId = localStorage.getItem('farmGuid') ;
    let lang = localStorage.getItem('lang');
    if(lang === LanguageConstant.EN) {
      this.localeSchedule = 'en-US'
    }
    if(lang === LanguageConstant.VI) {
      this.localeSchedule = 'vi'
    }
    if(lang === LanguageConstant.TW || lang === LanguageConstant.CN) {
      this.localeSchedule = 'zh'
    }
   
  }
  

  async ngModelChangeHall(value) {
    await this.loadRoomData(value)
    await this.loadWorkOrderData(value)
  }
  refresh(){
    this.hallDropdown.value = null
    this.roomDropdown.value = null
    this.funeralDropdown.value = null
    this.getPreviewScheduler()
  }
  search() {
    this.service.search(this.fiterRequest).subscribe((res: any) => {
      this.scheduleObj.eventSettings.dataSource =  res.map(x => {
        return {
          id: x.id,
          StartTime: x.startTime,
          EndTime: x.endTime,
          OrderName: x.orderName,
          HallName: x.hallName,
          RoomName: x.roomName,
          OrderNo: x.orderNo,
          Subject: x.subject,
          IsReadonly: true,
          CategoryColor: x.categoryColor
        }
      })
    })
  }
  async ngModelChangeHallFilter(args) {
    this.fiterRequest.hallGuidFiter = args.value
    this.search();
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

  public onClick(args) {
    if (!this.eventAdded) {
      let popupInstance = (document.querySelector(".e-quick-popup-wrapper") as any).ej2_instances[0];
      popupInstance.open = () => {
        popupInstance.refreshPosition();
      };
      this.eventAdded = true;
    }
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
              this.getPreviewScheduler();
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

  public onCloseClick(args): void {
    this.scheduleObj.quickPopup.quickPopupHide();
  }

  public buttonClickActions(args) {

  }

  getPreviewScheduler() {
    this.service.getPreviewScheduler(this.siteId).subscribe((res: any) => {
      this.scheduleObj.eventSettings.dataSource =  res.map(x => {
        return {
          id: x.id,
          StartTime: x.startTime,
          EndTime: x.endTime,
          OrderName: x.orderName,
          HallName: x.hallName,
          RoomName: x.roomName,
          OrderNo: x.orderNo,
          Subject: x.subject,
          IsReadonly: true,
          CategoryColor: x.categoryColor
        }
      })
      
    })
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
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
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
  getAudit(id) {
    this.service.getAudit(id).subscribe(data => {
      this.audit = data;
    });
  }
  loadFuneralDirector() {
    this.serviceXaccount.getXAccountsBySite(this.siteId).subscribe(res => {
      this.xAccountData = res
      this.xAccountData.unshift({ 
        id: 0, 
        guid: '', 
        accountName: this.translate.instant('DESKTOP_NO_ITEM_DATA') });
    })
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
      this.title = 'BOOKING_CALENDAR_EDIT_MODAL';
     } else {
       this.model.id = 0;
       this.model.funeralDirector = this.funeralGuildSystem
       this.title = 'BOOKING_CALENDAR_ADD_MODAL';
     }
     this.modalReference = this.modalService.open(template, {size: 'xl',backdrop: 'static'});
   }
  save() {
    if (this.model.id > 0) {
      this.update();
    } else {
      this.create();
    }
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
               this.getPreviewScheduler()
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
   
   ToDate(date: any) {
     if (date &&  date instanceof Date) {
       return this.datePipe.transform(date, "yyyy-MM-dd");
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
    public onPopupOpen(args: PopupOpenEventArgs): void {
      if (args.data.Id === undefined){
        args.cancel = true
        this.model = {} as any;
        this.start_time = new Date(new Date(this.ToDate(args.data.startTime)).getFullYear(), new Date(this.ToDate(args.data.startTime)).getUTCMonth(), new Date(this.ToDate(args.data.startTime)).getDate(),0,0)
        this.end_time = new Date(new Date(this.ToDate(args.data.startTime)).getFullYear(), new Date(this.ToDate(args.data.startTime)).getUTCMonth(), new Date(this.ToDate(args.data.startTime)).getDate(),23,59)
        this.openModal(this.templateRef);
      }
    }

    public onEventRendered(args: EventRenderedArgs): void {
      const categoryColor: string = args.data.CategoryColor as string;
      if (!args.element || !categoryColor) {
        return;
      }
      if (this.currentView === 'Agenda') {
        (args.element.firstChild as HTMLElement).style.borderLeftColor = categoryColor;
      } else {
        args.element.style.backgroundColor = categoryColor;
      }
    }

}
function setCulture(lang: string) {
  // throw new Error('Function not implemented.');
}

