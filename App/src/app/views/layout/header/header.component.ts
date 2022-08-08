import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { Component, OnInit, AfterViewInit } from '@angular/core';
import { AuthService } from '../../../_core/_service/auth.service';
import { AlertifyService } from '../../../_core/_service/alertify.service';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { FarmService } from 'src/app/_core/_service/farms';
import { DashboardService } from 'src/app/_core/_service/dashboard.service';
import { TranslateService } from '@ngx-translate/core';
import { HeaderService } from 'src/app/_core/_service/header.service';
import * as moment from 'moment';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit, AfterViewInit {
  nickName: any;
  public currentTime: any;
  username: any;
  farmData: any[];
  farmGuid: any;
  fields: object = { text: 'siteName', value: 'guid' };
  fieldsLang: object = { text: 'name', value: 'id' };
  lang: string;
  languageData = [{id: 'Tw', name: 'Tw'}, {id: 'Cn', name: 'Cn'}, {id: 'En', name: 'En'},{id: 'Vi', name: 'Vi'}]
  sysConf: any;
  constructor(
    private authService: AuthService,
    private cookieService: CookieService,
    private alertify: AlertifyService,
    private router:Router,
    private service: SiteService,
    private serviceDash: DashboardService,
    private trans: TranslateService,
    private serviceHeader: HeaderService,
    ) {
      
  }
  ngAfterViewInit(): void {
    
  }
  ngOnInit(): void {
    this.currentTime = moment().format('LTS');
    setInterval(() => this.updateCurrentTime(), 1 * 1000);
    this.sysConf = JSON.parse(localStorage.getItem('sysConf'))
    this.farmGuid = localStorage.getItem('farmGuid');
    this.lang = this.capitalize(localStorage.getItem('lang'));
    this.nickName =  JSON.parse(localStorage.getItem('user'))?.nickName || "No Name";
    this.username =  JSON.parse(localStorage.getItem('user'))?.username || "Guest";
    this.getFarmsByAccount();
    
  }
  updateCurrentTime() {
    this.currentTime = moment().format('LTS');
  }
  getFarmsByAccount() {
    this.service.getSiteByAccount().subscribe((data: any ) => {
      this.farmData = data;
      const farmGuid = localStorage.getItem('farmGuid');
      if (farmGuid) {
        this.farmGuid = farmGuid;
      } else {
        this.farmGuid = data[0]?.guid || "";
      }
      localStorage.setItem('farmGuid', this.farmGuid);
      this.serviceDash.changeFarmGuid(this.farmGuid)
    });
  }
  goToChangePassword() {
    this.router.navigate(['/change-password']);
  }
  goToProfile() {
    this.router.navigate(['/profile']);
  }
   logout() {
    this.authService.logOut().subscribe(() => {
      const uri = this.router.url;
      this.cookieService.deleteAll('/');

      this.router.navigate(['login'], { queryParams: { uri }, replaceUrl: true  });
      this.alertify.message(this.trans.instant('Logged out'));
    });
  }
  capitalize(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }

  farmValueChange(args) {
    this.farmGuid = args.itemData.guid || "";
    localStorage.setItem('farmGuid', args.itemData.guid);

    if (args.isInteracted === true) {
      location.reload();
    } else {
      setTimeout(() => {
      this.serviceDash.changeFarmGuid(this.farmGuid);
    }, 1000)

    }
  }
  langValueChange(args) {
    const lang = args.itemData.id.toLowerCase();
    localStorage.removeItem('lang');
    localStorage.setItem('lang', lang);
    this.lang = this.capitalize(localStorage.getItem('lang'));
    location.reload();
  }
}

