import { RoomDetail } from './../../../_core/_model/farms/roomDetail';
import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { DatePickerComponent } from '@syncfusion/ej2-angular-calendars';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { Room } from 'src/app/_core/_model/farms/room';
import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { HallService } from 'src/app/_core/_service/club-management/hall.service';
import { RoomService } from 'src/app/_core/_service/farms/room.service';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { environment } from 'src/environments/environment';
import { ActivatedRoute, Router } from '@angular/router';
import { UtilitiesService } from 'src/app/_core/_service/utilities.service';
import { ImagePathConstants } from 'src/app/_core/_constants';

@Component({
  selector: 'app-hall-detail',
  templateUrl: './hall-detail.component.html',
  styleUrls: ['./hall-detail.component.css']
})
export class HallDetailComponent extends BaseComponent implements OnInit {

  hallData: any;
  public dateValue: null;
  siteId: string = localStorage.getItem('farmGuid');
  hallFields: object = { text: 'hallName', value: 'guid' };
  fiterRequest = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
  }
  fiterRequestDefault = {
    siteGuid: this.siteId,
    hallGuidFiter: '',
    roomGuidFiter: '',
    funeralDirectorFilter: '',
  }
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  @ViewChild('roomDropdown') public roomDropdown: DropDownListComponent;
  @ViewChild('date') public date: DatePickerComponent;
  baseUrl = environment.apiUrl;
  roomData: any;
  roomFields: object = { text: 'roomName', value: 'guid' };
  data: any;
  model: RoomDetail = {} as RoomDetail;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE_ROOM;
  isImage: any;
  public noGuid: string = '';
  img1: any;
  constructor(
    private datePipe: DatePipe,
    private serviceHall: HallService,
    private serviceRoom: RoomService,
    private utilityService: UtilitiesService,
    private route: ActivatedRoute,
    private service: BookingService,
    public router: Router,
    public translate: TranslateService,
  ) { 
    super(translate);
  }

  ngOnInit(): void {
    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 5,
        arrowPrevIcon: 'fa fa-chevron-left',
        arrowNextIcon: 'fa fa-chevron-right',
        imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false
      }
    ];
    this.loadDataAsync(this.route.snapshot.params.roomID)
  }

 
  detailBooking() {
    if(this.route.snapshot.params.bookingID > 0) {
      this.router.navigate([`/mobile/booking/${this.route.snapshot.params.bookingID}`]);
    }else {
      this.router.navigate([`/mobile/booking/0`])
    }
  }
  async loadDataAsync(id) {
    this.loadHallData()
    if (id > 0) {
      const item = await this.getById(id);
      this.fiterRequest.hallGuidFiter = item.hallGuid
      await this.loadRoomData(item.hallGuid)
      this.model = {...item};
      this.fiterRequest.roomGuidFiter = item.roomGuid
      let listFileImage: string[] = [];
      listFileImage.push(this.model.roomPhoto1, this.model.roomPhoto2, this.model.roomPhoto3, this.model.roomPhoto4, this.model.roomPhoto5);
      this.galleryImages = listFileImage.filter(x => x !== null).map(x => {
        return {
          small: this.imagePath(x),
          medium:  this.imagePath(x),
          big:  this.imagePath(x)
        }
      })
    } else {
      this.model.id = 0;
    }
  }
 
  
  getById(id) {
    return this.serviceRoom.getRoomByid(id).toPromise();
  }
  refresh(){
    this.hallDropdown.value = null
    this.roomDropdown.value = null
    this.model.id =  0
    this.searchDefault()
  }
  loadData() {
    this.service.getBookingCheck().subscribe(res => {
      this.data = res
    })
  }
  imageExists(url, callback) {
    var img = new Image();
    img.onload = function() { callback(true); };
    img.onerror = function() { callback(false); };
    img.src = url;
  }
  checkIMG(path,number) {
    let that = this;
    let imageUrl = this.apiHost + path;
    if(path !== null) {
      this.imageExists(imageUrl, function(exists) {
        if(exists === false) {
          switch (number) {
            case 1:
              that.model.roomPhoto1 = that.noImage
              break;
            case 2:
              that.model.roomPhoto2 = that.noImage
              break;
            case 3:
              that.model.roomPhoto3 = that.noImage
              break;
            case 4:
              that.model.roomPhoto4 = that.noImage
              break;
            case 5:
              that.model.roomPhoto5 = that.noImage
              break;
          
            default:
              break;
          }
          
        }else {
          switch (number) {
            case 1:
              that.model.roomPhoto1 = path
              break;
            case 2:
              that.model.roomPhoto2 = path
              break;
            case 3:
              that.model.roomPhoto3 = path
              break;
            case 4:
              that.model.roomPhoto4 = path
              break;
            case 5:
              that.model.roomPhoto5 = path
              break;
          
            default:
              break;
          }
        }
      });
    }else {
      switch (number) {
        case 1:
          that.model.roomPhoto1 = that.noImage
          break;
        case 2:
          that.model.roomPhoto2 = that.noImage
          break;
        case 3:
          that.model.roomPhoto3 = that.noImage
          break;
        case 4:
          that.model.roomPhoto4 = that.noImage
          break;
        case 5:
          that.model.roomPhoto5 = that.noImage
          break;
      
        default:
          break;
      }
    }
    return this.img1
  }
  search() {
    this.service.searchRoomDetail(this.fiterRequest).subscribe((res: any) => {
      this.model = {...res};
      if(res === null) {
        this.model.id = 0
      }
      this.checkIMG(this.model.roomPhoto1,1);
      this.checkIMG(this.model.roomPhoto2,2);
      this.checkIMG(this.model.roomPhoto3,3);
      this.checkIMG(this.model.roomPhoto4,4);
      this.checkIMG(this.model.roomPhoto5,5);
      setTimeout(() => {
        let listFileImage: string[] = [];
        listFileImage.push(this.model.roomPhoto1, this.model.roomPhoto2, this.model.roomPhoto3, this.model.roomPhoto4, this.model.roomPhoto5);
        this.galleryImages = listFileImage.filter(x => x !== null).map(x => {
          return {
            small: this.imagePath(x),
            medium:  this.imagePath(x),
            big:  this.imagePath(x)
          }
        })
      }, 300);
    })
  }

  searchDefault() {
    this.service.searchRoomDetail(this.fiterRequestDefault).subscribe((res: any) => {
      this.model = {...res};
      if(res === null) {
        this.model.id = 0
      }
      this.checkIMG(this.model.roomPhoto1,1);
      this.checkIMG(this.model.roomPhoto2,2);
      this.checkIMG(this.model.roomPhoto3,3);
      this.checkIMG(this.model.roomPhoto4,4);
      this.checkIMG(this.model.roomPhoto5,5);
      setTimeout(() => {
        let listFileImage: string[] = [];
        listFileImage.push(this.model.roomPhoto1, this.model.roomPhoto2, this.model.roomPhoto3, this.model.roomPhoto4, this.model.roomPhoto5);
        this.galleryImages = listFileImage.filter(x => x !== null).map(x => {
          return {
            small: this.imagePath(x),
            medium:  this.imagePath(x),
            big:  this.imagePath(x)
          }
        })
      }, 300);
    })
  }
  
  checkImage(path) {
    return new Promise((res, rej) => {
      this.service.checkImage(path).subscribe(
        (result) => {
          this.isImage = result
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  imagePath(path) {
    if (path !== null && this.utilityService.checkValidImage(path)) {
      if (this.utilityService.checkExistHost(path)) {
        return path;
      }
      return this.apiHost + path;
    }
    return this.noImage;
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
    // this.fiterRequest.bookingDate = this.datePipe.transform(args.value , 'yyyy-MM-dd') 
  }
  async ngModelChangeHallFilter(args) {
    if(args.isInteracted) {
      this.fiterRequest.hallGuidFiter = args.value
      this.search();
      await this.loadRoomData(args.value)
    }

  }

  async ngModelChangeRoomFilter(args) {
    if(args.isInteracted) {
      this.fiterRequest.roomGuidFiter = args.value
      this.search();
    }
  }

  loadRoomData(hallID) {
    return new Promise((res, rej) => {
      this.serviceRoom.getRoomBySiteAndHall(this.siteId,hallID).subscribe(
        (result) => {
          this.roomData = result
          this.roomData.unshift({ 
            id: 0, 
            guid: 'noGuid', 
            roomName: this.translate.instant('MOBILE_NO_ITEM_DATA') });
          this.noGuid = 'noGuid'
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

}
