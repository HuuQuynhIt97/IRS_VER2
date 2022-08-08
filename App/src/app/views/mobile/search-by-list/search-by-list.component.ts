import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { DatePickerComponent } from '@syncfusion/ej2-angular-calendars';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { LanguageConstant } from 'src/app/_core/_constants/language.constant';
import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';

@Component({
  selector: 'app-search-by-list',
  templateUrl: './search-by-list.component.html',
  styleUrls: ['./search-by-list.component.css']
})
export class SearchByListComponent extends BaseComponent implements OnInit {
  hallData: any;
  public dateValue: Date = new Date();
  siteId: string = localStorage.getItem('farmGuid');
  hallFields: object = { text: 'hallName', value: 'guid' };
  fiterRequest = {
    siteGuid: this.siteId,
    siteGuidFilter: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: this.datePipe.transform(this.dateValue , 'yyyy-MM-dd')
  }

  fiterRequestDefault = {
    siteGuid: this.siteId,
    siteGuidFilter: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
    bookingDate: this.datePipe.transform(this.dateValue , 'yyyy-MM-dd')
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
    public translate: TranslateService,
  ) { 
    super(translate);
  }

  ngOnInit(): void {
    // this.loadData()
    this.loadHallData()
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
  }
  refresh(){
    this.hallDropdown.value = null
    this.fiterRequest.bookingDate = this.datePipe.transform(new Date() , 'yyyy-MM-dd')
    this.date.value = new Date()
    this.searchDefault()
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
  detailRoom(roomID,bookingID) {
    this.router.navigate([`/mobile/hall-detail/${roomID}/${bookingID}`]);
  }
  search() {
    return new Promise((res, rej) => {
      this.service.getSearchByList(this.fiterRequest).subscribe(
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
      this.service.getSearchByList(this.fiterRequestDefault).subscribe(
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
    this.search();
  }
  async ngModelChangeHallFilter(args) {
    this.fiterRequest.hallGuidFiter = args.value
    await this.search();
    // await this.loadRoomData(value)
  }

}
