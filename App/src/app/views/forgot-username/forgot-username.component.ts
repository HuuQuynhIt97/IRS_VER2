import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { MessageConstants } from 'src/app/_core/_constants';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { AuthService } from 'src/app/_core/_service/auth.service';

@Component({
  selector: 'app-forgot-username',
  templateUrl: './forgot-username.component.html',
  styleUrls: ['./forgot-username.component.scss']
})
export class ForgotUsernameComponent implements OnInit {
  email: string;
  loading: number;
  alert = {
    send_mail_ok_msg: MessageConstants.SEND_MAIL_OK_MSG,
    send_mail_failed_msg: MessageConstants.SEND_MAIL_FAILED_MSG,
  };
  constructor(
    private auth: AuthService,
    private alertify: AlertifyService,
    private translate: TranslateService
    ) {    
      this.getAlertTranslator();
    }
    getAlertTranslator() {
      
  
      this.translate.get(this.alert.send_mail_failed_msg).subscribe((res: string) => {
        if (res) {
          this.alert.send_mail_failed_msg = res;
        }
      });
      this.translate.get(this.alert.send_mail_ok_msg).subscribe((res: string) => {
        if (res) {
          this.alert.send_mail_ok_msg = res;
        }
      });
    }
  ngOnInit() {
  }
  submit() {
    this.loading = 1;
    this.auth.forgotUsername({email: this.email}).subscribe((res:any)=> {
      this.loading = 0;
      if (res.success) {
        this.alertify.success(this.alert.send_mail_ok_msg, true);
      } else {
        this.alertify.warning(this.alert.send_mail_failed_msg, true);

      }
    },(err) => {
      this.loading = 0;
      this.alertify.warning(this.alert.send_mail_failed_msg, true);

    })
  }
}


