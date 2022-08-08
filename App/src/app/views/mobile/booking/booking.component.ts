import { RoomDetail } from './../../../_core/_model/farms/roomDetail';
import { DatePipe } from '@angular/common';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DatePickerComponent, DateTimePickerComponent } from '@syncfusion/ej2-angular-calendars';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Room } from 'src/app/_core/_model/farms/room';
import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';
import { RoomService } from 'src/app/_core/_service/farms/room.service';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { environment } from 'src/environments/environment';
import { BookingDetail } from 'src/app/_core/_model/booking/bookingDetail';
import { WorkorderService } from 'src/app/_core/_service/booking/workorder.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LanguageConstant } from 'src/app/_core/_constants/language.constant';

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent extends BaseComponent implements OnInit {

  funeralGuildSystem = JSON.parse(localStorage.getItem('user'))?.guid
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
  bookingID: number = 0
  dateLocale: string = 'en-US'
  @ViewChild('fisrtdatetime') public fisrtdatetime: DateTimePickerComponent;
  constructor(
    private alertify: AlertifyService,
    public modalService: NgbModal,
    private service: BookingService,
    private serviceHall: HallService,
    private datePipe: DatePipe,
    private serviceRoom: RoomService,
    private serviceWorkOrder: WorkorderService,
    private serviceXaccount: XAccountService,
    private route: ActivatedRoute,
    private router: Router,
    public translate: TranslateService
  ) {super(translate); }

  async ngOnInit() {
    // this.fisrtdatetime.timeFormat = 'HH:mm'
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
    await this.loadFuneralDirector()
    this.loadDataAsync(this.route.snapshot.params.id)
  }
  async loadDataAsync(id) {
      this.loadHallData()
     if (id > 0) {
      const item = await this.getById(id);
      await this.loadRoomData(item.hallGuid)
      await this.loadWorkOrderData(item.hallGuid)
      this.model = {...item};
     } else {
       this.model.id = 0;
       this.model.funeralDirector = this.funeralGuildSystem
     }
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
  cancel() {
    this.router.navigateByUrl('/mobile/booking-history');
  }

  async ngModelChangeHall(value) {
    await this.loadRoomData(value)
    await this.loadWorkOrderData(value)
  }
  refresh(){
    this.hallDropdown.value = null
    this.roomDropdown.value = null
    this.funeralDropdown.value = null
  }
  async ngModelChangeHallFilter(value) {
    await this.loadRoomData(value)
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

  public buttonClickActions(args) {

  }
  
  loadHallData() {
    return new Promise((res, rej) => {
      this.serviceHall.loadDataBySiteGuiId(this.siteId).subscribe(
        (result) => {
          this.hallData = result
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
    return new Promise((res, rej) => {
      this.serviceXaccount.getXAccountsBySite(this.siteId).subscribe(
        (result) => {
          this.xAccountData = result
          this.model.funeralDirector = this.funeralGuildSystem
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
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
               this.router.navigateByUrl('/mobile/booking-history');
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
               this.router.navigateByUrl('/mobile/booking-history');
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
