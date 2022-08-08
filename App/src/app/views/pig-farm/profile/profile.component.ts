import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ImagePathConstants, MessageConstants } from 'src/app/_core/_constants';
import { Profile } from 'src/app/_core/_model/xaccount';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { UtilitiesService } from 'src/app/_core/_service/utilities.service';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { environment } from 'src/environments/environment';
declare let $: any;
import { DataManager, Query, ODataV4Adaptor } from '@syncfusion/ej2-data';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit,AfterViewInit {
  apiHost = environment.apiUrl.replace('/api/', '');
  noImage = ImagePathConstants.NO_IMAGE;
  file: any;
  data: Profile = {} as Profile;
  option:any;
  alert = {
    updateMessage: this.trans.instant(MessageConstants.UPDATE_MESSAGE),
    updateTitle:  this.trans.instant(MessageConstants.UPDATE_TITLE),
    createMessage: this.trans.instant(MessageConstants.CREATE_MESSAGE),
    createTitle:  this.trans.instant(MessageConstants.CREATE_TITLE),
    deleteMessage:  this.trans.instant(MessageConstants.DELETE_MESSAGE),
    deleteTitle:  this.trans.instant(MessageConstants.DELETE_TITLE),
    cancelMessage:  this.trans.instant(MessageConstants.CANCEL_MESSAGE),
    serverError:  this.trans.instant(MessageConstants.SERVER_ERROR),
    deleted_ok_msg:  this.trans.instant(MessageConstants.DELETED_OK_MSG),
    created_ok_msg:  this.trans.instant(MessageConstants.CREATED_OK_MSG),
    updated_ok_msg:  this.trans.instant(MessageConstants.UPDATED_OK_MSG),
    system_error_msg:  this.trans.instant(MessageConstants.SYSTEM_ERROR_MSG),
    exist_message: this.trans.instant( MessageConstants.EXIST_MESSAGE),
    yes_message:  this.trans.instant(MessageConstants.YES_MSG),
    no_message:  this.trans.instant(MessageConstants.NO_MSG),

  };

  public dataSource: any;
  public query: Query ;
  public remoteFields: Object = { text: 'name', value: 'guid' };
  baseUrl = environment.apiUrl;
  public onFiltering: any = (e: any) => {
    let query = new Query();
    //frame the query based on search string with filter type.
    query = (e.text != "") ? query.where("name", "contains", e.text, true) : query;
    //pass the filter data source, filter query to updateData method.
    e.updateData(this.dataSource, query);
  };
  pageSizeSetting: any;
  constructor(
    private utilityService: UtilitiesService,
    private alertify: AlertifyService,
    private service: XAccountService,
    private trans: TranslateService,
    ) { }

  ngOnInit() {
    this.query = new Query()
    .skip(0)
    .take(50)
    .addParams('farmGuid', localStorage.getItem('farmGuid'))
    .addParams('codeType', "PageSize_Setting")
    .addParams('lang', localStorage.getItem('lang'));
     new DataManager({
      url: `${this.baseUrl}CodeType/GetCodeTypes`,
      adaptor: new ODataV4Adaptor,
      crossDomain: true,
    }).executeQuery(this.query).then((event: any) => {
      this.dataSource = event.result as { [key: string]: Object }[];
    }).catch((e: Object) => {
      this.dataSource = [] as { [key: string]: Object }[];
    });

    this.data = {} as Profile;
    this.data.pageSizeSetting = "";
    const key = JSON.parse(localStorage.getItem('user'))?.id;
    this.option = {
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
      defaultPreviewContent: '<img src="/assets/images/no-img.jpg" alt="no image">',
      layoutTemplates: { main2: '{preview} ' + ' {browse}' },
      allowedFileExtensions: ["jpg", "png", "gif"],
      initialPreview: [],
      deleteUrl: `${environment.apiUrl}XAccount/DeleteUploadFile`,
      uploadUrl: `${environment.apiUrl}XAccount/UploadAvatar?key=${key}`,
      initialPreviewConfig: [{
        caption: '',
        width: '',
        url: `${environment.apiUrl}XAccount/DeleteUploadFile`, // server delete action
        key: key
      }]
    };

    this.getProfile();
  }
  getProfile() {
    const key = JSON.parse(localStorage.getItem('user'))?.guid;
    this.service.getProfile(key).subscribe(data => {
      this.data = data;
      setTimeout(() => {
      this.pageSizeSetting = this.data.pageSizeSetting;
      }, 500);
        if (this.data.photoPath) {
          this.data.photoPath = this.imagePath(this.data.photoPath);
          const img = `<img src='${this.data.photoPath}' class='file-preview-image' alt='img' title='img'>`;
          this.option.initialPreview = [img]
        }
        $("#avatar-1").fileinput(this.option);
        let that = this;
        $('#avatar-1').on('filedeleted', function (event, key, jqXHR, data) {
          that.data.photoPath = null;
          that.option.initialPreview = [];
          that.option.initialPreviewConfig = [];
          $(this).fileinput(that.option);
        });
        $('#avatar-1').on('fileuploaded', function(event, previewId, index, fileId) {

       });
    }, () => {
      $("#avatar-1").fileinput(this.option);
    });

   }
  ngAfterViewInit(): void {


  }

  onFileChangeLogo(args) {
    this.file = args.target.files[0];
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
  storeProfile() {
    this.alertify.confirm4(
      this.alert.yes_message,
      this.alert.no_message,
      this.alert.updateTitle,
      this.alert.updateMessage,
      () => {
        this.data.pageSizeSetting = this.pageSizeSetting;
        this.service.storeProfile(this.data).subscribe(
          (res) => {
            if (res.success === true) {
              this.alertify.success(this.alert.updated_ok_msg);
              this.getProfile();
              let user = JSON.parse(localStorage.getItem('user'));
              user.pageSizeSettingValue = res.data.pageSizeSettingValue;
              user.pageSizeSetting = res.data.pageSizeSetting;
              localStorage.setItem('user', JSON.stringify(user));
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
