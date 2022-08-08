import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { AuthService } from 'src/app/_core/_service/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  newPassword: string;
  confirmPassword: string;
  token: string;
  status1 = false;
  status2 = false;
  loading: number;
  constructor(
    private auth: AuthService,
    private alertify: AlertifyService,
    private activatedRoute: ActivatedRoute,
  ) { }

  ngOnInit() {
      this.activatedRoute.queryParams.subscribe(params => {

      this.token =  params['token'];
        console.log(this.token );
      });
  }
  submit() {
    this.loading = 1;

    if (!this.newPassword) {
      this.alertify.warning('Enter new password', true);
      return;
    }

    if (!this.confirmPassword) {
      this.alertify.warning('Enter confirm password', true);
      return;
    }
    if (this.newPassword !== this.confirmPassword) {
      this.alertify.warning('New password and confirm password does not match', true);
      return;
    }
    const model = {
      token: this.token,
      newPassword: this.newPassword
    }
    this.auth.resetPassword(model).subscribe((res: any)=> {
      this.loading = 0;
      this.alertify.success(res.message, true);
    }, (err) => {
      this.loading = 0;
      this.alertify.error(err, true);
    })
  }
}
