import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DatePickerComponent } from '@syncfusion/ej2-angular-calendars';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { LanguageConstant } from 'src/app/_core/_constants/language.constant';
import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';

@Component({
  selector: 'app-booking-history',
  templateUrl: './booking-history.component.html',
  styleUrls: ['./booking-history.component.css']
})
export class BookingHistoryComponent extends BaseComponent implements OnInit {

  hallData: any;
  public dateValue: null;
  siteId: string = localStorage.getItem('farmGuid');
  hallFields: object = { text: 'hallName', value: 'guid' };
  fiterRequest = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: null
  }
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  @ViewChild('date') public date: DatePickerComponent;
  data: any;
  noGuid: string = '';
  dateLocale: string = 'en-US'
  constructor(
    private datePipe: DatePipe,
    private serviceHall: HallService,
    private service: BookingService,
    public router: Router,
    private route: ActivatedRoute,
    public translate: TranslateService,
  ) { 
    super(translate);
  }

  ngOnInit(): void {
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
    this.loadData()
    this.loadHallData()
  }
  refresh(){
    this.hallDropdown.value = null
    this.fiterRequest.bookingDate = null
    this.date.value = null
    this.loadData()
  }
  detailBooking(bookingID) {
    this.router.navigate([`/mobile/booking/${bookingID}`]);
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
  search() {
    this.service.searchBookingCheck(this.fiterRequest).subscribe((res: any) => {
      this.data = res
    })
  }
  loadHallData() {
    return new Promise((res, rej) => {
      this.serviceHall.loadDataBySiteGuiId(this.siteId).subscribe(
        (result) => {
          this.hallData = result
          this.hallData.unshift({ 
            id: 0, 
            guid: 'noGuid', 
            hallName: this.translate.instant('MOBILE_NO_ITEM_DATA') });
          this.noGuid = 'noGuid'
        },
        (error) => {
          rej(error);
        }
      );
    });
  }
  changeTime(args) {
    this.fiterRequest.bookingDate = this.datePipe.transform(args.value , 'yyyy-MM-dd') 
  }
  async ngModelChangeHallFilter(value) {
    // await this.loadRoomData(value)
  }

}
