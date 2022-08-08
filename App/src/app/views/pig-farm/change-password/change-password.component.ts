import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/_core/_service/account.service';
import { MessageConstants } from 'src/app/_core/_constants';
import { XAccountService } from 'src/app/_core/_service/xaccount.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  confirmPassword: any;
  newPassword: any;
  status = false;
  status1 = false;
  status2 = false;
  oldPassword: any;
  alert = {
    system_error_msg: MessageConstants.SYSTEM_ERROR_MSG,
    updated_ok_msg: MessageConstants.UPDATED_OK_MSG,
    exist_message: MessageConstants.EXIST_MESSAGE,
    valid_change_password_msg: MessageConstants.VALID_CHANGE_PASSWORD_MSG,
    config_change_password_msg: MessageConstants.CONFIRM_CHANGE_PASSWORD_MSG

  };
  constructor(
    private alertify: AlertifyService,
    private service: XAccountService,
    private translate: TranslateService,
    private router: Router
  ) { }

  ngOnInit() {
  }

  submit() {
    if (!this.newPassword || !this.confirmPassword) {
      this.alertify.warning(this.alert.valid_change_password_msg, true);
      return;
    }
    if (this.newPassword !== this.confirmPassword) {
      this.alertify.warning(this.alert.config_change_password_msg, true);
      return;
    }
    const request = {
      id: +JSON.parse(localStorage.getItem("user")).id,
      upwd: this.newPassword,
      oldPassword: this.oldPassword
    }
    this.service.changePassword(request).subscribe( res => {
      if (res.success === true) {
        this.alertify.success(this.alert.updated_ok_msg);
        this.newPassword = '';
        this.confirmPassword = '';
        this.oldPassword = '';
      } else {
        this.translate.get(res.message).subscribe((data: string) => {
          this.alertify.warning(data, true);
        });
      }
    }, err => {
      this.alertify.warning(this.alert.system_error_msg)
    })
  }
}
