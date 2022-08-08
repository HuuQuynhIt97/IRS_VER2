import { BookingService } from 'src/app/_core/_service/booking/booking.service';
import { HallService } from './../../../../_core/_service/club-management/hall.service';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { HallScreen } from './../../../../_core/_model/club-management/hall';
import { SiteScreen } from './../../../../_core/_model/club-management/site';
import { RoomService } from './../../../../_core/_service/club-management/room.service';
import { Room, RoomScreen } from './../../../../_core/_model/club-management/room';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
import { L10n, setCulture } from '@syncfusion/ej2-base';
import { Component, EventEmitter, OnInit, Output, TemplateRef, ViewChild, OnDestroy, Input } from '@angular/core';
import { NgbModalRef, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { ImagePathConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BaseComponent } from 'src/app/_core/_component/base.component';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DatePipe } from '@angular/common';
import { UtilitiesService } from 'src/app/_core/_service/utilities.service';
import { FunctionUtility } from 'src/app/_core/_helper/function-utility';
import { DropDownListComponent } from '@syncfusion/ej2-angular-dropdowns';
declare let $: any;

@Component({
  selector: 'app-room-list-setting',
  templateUrl: './room-list-setting.component.html',
  styleUrls: ['./room-list-setting.component.css']
})
export class RoomListSettingComponent extends BaseComponent implements OnInit, OnDestroy {

  @Input() height;
  localLang = (window as any).navigator.userLanguage || (window as any).navigator.language;
  @Output() selectRoom = new EventEmitter();
  data: DataManager;
  baseUrl = environment.apiUrl;
  modalReference: NgbModalRef;
  @ViewChild('grid') public grid: GridComponent;
  model: Room = {} as Room;
  setFocus: any;
  locale = localStorage.getItem('lang');
  editSettings = { showDeleteConfirmDialog: false, allowEditing: false, allowAdding: true, allowDeleting: false, mode: 'Normal' };
  title: any;
  @ViewChild('optionModal') templateRef: TemplateRef<any>;
  toolbarOptions = ['Add', 'Search'];
  selectionOptions = { checkboxMode: 'ResetOnRowClick' };
  subscriptions: Subscription[] = [];
  site: SiteScreen;
  hall: HallScreen;
  room: RoomScreen;
  roomPhoto: any;
  loading = 0;
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE_ROOM;
  listFile: File[] = [];
  listFileImage: string[];
  hallData: any;
  hallFields: object = { text: 'hallName', value: 'guid' };

  siteData: any;
  siteFields: object = { text: 'siteName', value: 'guid' };
  fiterRequest = {
    siteGuidFilter: '',
    hallGuidFilter: ''
  }
  fiterRequestDefault = {
    siteGuidFilter: '',
    hallGuidFilter: ''
  }
  name: string;
  @ViewChild('siteDropdown') public siteDropdown: DropDownListComponent;
  @ViewChild('hallDropdown') public hallDropdown: DropDownListComponent;
  isImage: boolean;
  img1: string;
  constructor(
    private service: RoomService,
    private serviceSite: SiteService,
    private serviceHall: HallService,
    private bookingService: BookingService,
    public modalService: NgbModal,
    private alertify: AlertifyService,
    private datePipe: DatePipe,
    private utilityService: UtilitiesService,
    public translate: TranslateService,
    private functionUtility: FunctionUtility
  ) { super(translate); }
  ngOnDestroy(): void {
    this.subscriptions?.forEach(item => item.unsubscribe());
  }
  change(args: any) {
    this.pageSettings.currentPage = args.value;
  }
  ngOnInit() {
    // this.Permission(this.route);
    let lang = localStorage.getItem('lang');
    let languages = JSON.parse(localStorage.getItem('languages'));
    setCulture(lang);
    let load = {
      [lang]: {
        grid: languages['grid'],
        pager: languages['pager']
      }
    };
    L10n.load(load);
    this.getAsyncData()
    // Sample usage
  
  }
  
  
  

  searchGrid(args) {
    this.grid.search(this.name)
  }
  async getAsyncData() {
    await this.loadData()
    await this.loadSiteData()
  }
  loadData() {
    return new Promise((res, rej) => {
      this.service.getAll().subscribe(
        (result: any) => {
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
            guid: '', 
            siteName: this.translate.instant('DESKTOP_NO_ITEM_DATA') });
          res(result);
        },
        (error) => {
          rej(error);
        }
      );
    });
  }

  loadHallData(siteID) {
    return new Promise((res, rej) => {
      this.serviceHall.loadDataBySiteGuiId(siteID).subscribe(
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
  search() {
    this.service.search(this.fiterRequest).subscribe((res: any) => {
      this.data = res
    })
  }

  searchDefault() {
    this.service.search(this.fiterRequestDefault).subscribe((res: any) => {
      this.data = res
    })
  }
  refresh() {
    this.siteDropdown.value = null
    this.hallDropdown.value = null
    this.searchDefault()
  }
  async ngModelChangeSiteFilter(args) {
    this.fiterRequest.siteGuidFilter = args.value
    this.search();
    await this.loadHallData(args.value)
  }
  async ngModelChangeHallFilter(args) {
    this.fiterRequest.hallGuidFilter = args.value
    this.search();
  }
  dataBound() {
    // this.grid.autoFitColumns();
  }
  typeChange(value) {
    this.model.type = value || "";
  }
  // life cycle ejs-grid
  rowSelected(args) {
  }
  recordClick(args: any) {
    this.service.changeRoom(args.rowData);

  }
  onDoubleClick(args: any): void {
    this.setFocus = args.column; // Get the column from Double click event
  }

  onChange(args, data) {
    data.isDefault = args.checked;
  }

  actionBegin(args) {
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
  actionComplete(args) {
    // if (args.requestType === 'add') {
    //   args.form.elements.namedItem('name').focus(); // Set focus to the Target element
    // }
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
  create() {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.createTitle,
      this.alert.createMessage,
      () => {
        this.model.siteGuid = this.site.guid;
        this.model.hallGuid = this.hall.guid;
        this.model.file = this.roomPhoto || [];
        delete this.model['column'];
        delete this.model['index'];
        this.model.roomGallery = this.listFile;
        this.service.insertForm(this.model).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.created_ok_msg);
              this.loadData();
              this.modalReference.dismiss();
              this.listFile = [];
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
  ToDate(date: any) {
    if (date && date instanceof Date) {
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
        this.model.file = this.roomPhoto || [];
        delete this.model['column'];
        delete this.model['index'];
        this.model.siteGuid = this.site.guid;
        this.model.hallGuid = this.hall.guid;
        this.model.roomGallery = this.listFile;
        this.service.updateForm(this.model as Room).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.loadData();
              this.modalReference.dismiss();
              this.listFile = [];
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
  // end api
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }
  onFileChangeLogo(args) {
    this.roomPhoto = args.target.files[0];
  }
  save() {
    if (this.model.id > 0) {
      this.update();
    } else {
      this.create();
    }
  }
  getById(id) {
    return this.service.getById(id).toPromise();
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
              status = exists
              that.model.roomPhoto1 = that.noImage
              break;
            case 2:
              status = exists
              that.model.roomPhoto2 = that.noImage
              break;
            case 3:
              status = exists
              that.model.roomPhoto3 = that.noImage
              break;
            case 4:
              status = exists
              that.model.roomPhoto4 = that.noImage
              break;
            case 5:
              status = exists
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
  async openModal(template, data = {} as Room) {

    if (data?.id > 0) {
      const item = await this.getById(data.id);
      this.model = { ...item };
      this.imagePath(item.roomPhoto);
      this.checkIMG(this.model.roomPhoto1,1);
      this.checkIMG(this.model.roomPhoto2,2);
      this.checkIMG(this.model.roomPhoto3,3);
      this.checkIMG(this.model.roomPhoto4,4);
      this.checkIMG(this.model.roomPhoto5,5);
      this.getAudit(this.model.id);
      this.title = 'ROOM_EDIT_MODAL';
      // ***Here is the code for converting "String" to "File"
      setTimeout(() => {
        let listFileImage: string[] = [];
        listFileImage.push(this.model.roomPhoto1, this.model.roomPhoto2, this.model.roomPhoto3, this.model.roomPhoto4, this.model.roomPhoto5);
        listFileImage = listFileImage.filter(item => item !== null);
        if (listFileImage?.length > 0)
          this.functionUtility.convertToFile(listFileImage, this.apiHost, this.listFile);
      }, 300);
    } else {
      this.model.id = 0;

      this.title = 'ROOM_ADD_MODAL';
    }
    this.modalReference = this.modalService.open(template, { size: 'xl', backdrop: 'static' });
    setTimeout(() => {
      let img = `<img src='${this.model.roomPhoto}' class='file-preview-image' alt='Desert' title='Desert'>`;
      const option = {
        overwriteInitial: true,
        maxFileSize: 1500,
        showClose: false,
        showCaption: false,
        browseLabel: '',
        removeLabel: '',
        browseIcon: '<i class="bi-folder2-open"></i>',
        removeIcon: '<i class="bi-x-lg"></i>',
        removeTitle: 'Cancel or reset changes',
        elErrorContainer: '#kv-avatar-errors-1',
        msgErrorClass: 'alert alert-block alert-danger',
        defaultPreviewContent: img,
        layoutTemplates: { main2: '{preview} ' + ' {browse}' },
        allowedFileExtensions: ["jpg", "png", "gif"],
        initialPreview: [],
        initialPreviewConfig: [],
        deleteUrl: `${environment.apiUrl}room/DeleteUploadFile`
      };
      if (this.model.roomPhoto) {
        const a = {
          caption: '',
          width: '',
          url: `${environment.apiUrl}Room/DeleteUploadFile`, // server delete action
          key: this.model.id,
          extra: { id: this.model.id }
        }
        option.initialPreviewConfig = [a];
      }
      $("#avatar-1").fileinput(option);;
      let that = this;
      $('#avatar-1').on('filedeleted', function (event, key, jqXHR, data) {
        that.roomPhoto = null;
        that.model.file = null;
        that.model.roomPhoto = null;
        option.initialPreview = [];
        option.initialPreviewConfig = [];
        $(this).fileinput(option);
  
      });
    }, 400);
  }
  // checkImage(path) {
  //   this.bookingService.checkImage(path).subscribe(res => {
  //     if(res === false) {
  //       this.model.roomPhoto = this.noImage
  //     }else {
  //       this.model.roomPhoto = this.imagePath(path)
  //     }
  //   })
    
  // }
  closeModal() {
    this.listFile = [];
    this.modalReference.close();
  }

  imagePathDefault(path) {
    if (path !== null && this.utilityService.checkValidImage(path)) {
      if (this.utilityService.checkExistHost(path)) {
        return path;
      }
      return this.apiHost + path;
    }
    return this.noImage;
  }
  imagePath(path) {
    if (path !== null ) {
      this.bookingService.checkImage(path).subscribe(res => {
        if(res === false) {
          return this.model.roomPhoto = this.noImage
        }else {
          return this.model.roomPhoto = this.apiHost + path
        }
      })
    }else {
      this.model.roomPhoto = this.noImage
    }
  }

  onSelect(event) {
    let fileSize = 0;
    let length = 0;
    if(this.listFile !== null) {
      length = this.listFile.length;
      this.listFile.forEach(item => {
        fileSize += item.size;
      });
    }
    // Kiểm tra tổng dung lượng của tất cả file import
    if (event.addedFiles && event.addedFiles[0]) {
      length += event.addedFiles.length;
      if (length > 5) {
        this.alertify.warning("You can upload 5 files images", true);
        return;
      }

      event.addedFiles.forEach(element => {
        fileSize += element.size;
      });
      if (fileSize > 26214400) {
        return this.alertify.warning("Sum all image file size upload can't more than 25MB", true);
      }
      else {
        this.listFile.push(...event.addedFiles);
      }
    }
  }

  onRemove(event) {
    this.listFile.splice(this.listFile.indexOf(event), 1);
  }

}
