import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { CookieService } from 'ngx-cookie-service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-p404',
  templateUrl: './p404.component.html',
  styleUrls: ['./p404.component.css']
})
export class P404Component implements OnInit {
  title: string;
  btnText: string;

  constructor(
    private router: Router,
    private alertify: AlertifyService,
    private translate: TranslateService,
    private cookieService: CookieService
  ) { }

  ngOnInit(): void {
    this.loadLang();
    this.alertify.errorBackToLogin(this.title, this.btnText, () => {
      this.cookieService.deleteAll('/');
      localStorage.removeItem('user');
      localStorage.removeItem('token');
      localStorage.removeItem('refresh-token');
      localStorage.removeItem('login-event');
      localStorage.removeItem('functions');
      localStorage.removeItem('menuItem');
      localStorage.removeItem('farmGuid');
      localStorage.removeItem('menus');
      this.router.navigateByUrl('/login');
    });
  }
  loadLang() {
    this.translate.get("Access-denied").subscribe(res => {
      this.title = res;
    });
    this.translate.get("Back to login").subscribe(res => {
      this.btnText = res;
    });
  }
}
