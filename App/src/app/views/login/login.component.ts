
import { Component, OnDestroy, OnInit, AfterViewInit } from '@angular/core';
import { AlertifyService } from '../../_core/_service/alertify.service';
import { Router, ActivatedRoute } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { UserForLogin } from 'src/app/_core/_model/user';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/_core/_service/auth.service';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { TranslateService } from '@ngx-translate/core';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy, AfterViewInit {
  username = '';
  password = '';
  private subscription: Subscription;
  user: UserForLogin = {
    username: '',
    password: ''
  };
  uri: any;
  remember = false;
  loading = 0;
  key: string;
  sysConf: any;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService,
    private cookieService: CookieService,
    private alertifyService: AlertifyService,
    private trans: TranslateService
  ) {
    if (this.cookieService.get('remember') !== undefined) {
      if (this.cookieService.get('remember') === 'Yes') {
        this.key = this.cookieService.get('key_temp');
        this.remember = true;
        this.loginRememberMe(+this.key);
      }
    }
    let backUrl = '/dashboard';
    this.uri = this.route.snapshot.queryParams.uri || backUrl;
  }
  role: number;
  ngAfterViewInit(): void {

  }
  ngOnInit(): void {
    const accessToken = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refresh_token');
    if (accessToken && refreshToken && this.route.routeConfig.path === 'login') {
      let backUrl = '/dashboard';
      const uri = decodeURI(this.uri) || backUrl;
      this.router.navigate([uri]);
    }
    this.sysConf = JSON.parse(localStorage.getItem('sysConf'))
    if (this.authService.loggedIn()) {
      let backUrl = '/dashboard';
      const uri = decodeURI(this.uri) || backUrl;
      this.router.navigate([uri]);
    }
  }
  onChangeRemember(args) {
    this.remember = args.target.checked;
  }
  authentication() {
    return this.authService
      .login(this.username, this.password).toPromise();
  }

  async login() {
    if (!this.username || !this.password) {
      return;
    }
    this.loading = 1;
    try {
      const data = await this.authentication();
      const currentLang = localStorage.getItem('lang');
      if (currentLang) {
        localStorage.setItem('lang', currentLang);
      } else {
        localStorage.setItem('lang', 'tw');
      }

      if (this.remember) {
        this.cookieService.set('remember', 'Yes');
        this.cookieService.set('key_temp', data.user.id);
      } else {
        this.cookieService.set('remember', 'No');
        this.cookieService.set('key_temp', '');
      }
      const lang = localStorage.getItem('lang');

      const functions = await this.authService.getPermissions(data.user.guid,lang ).toPromise();
      localStorage.setItem("functions", JSON.stringify(functions));

      const languages = await this.authService.getLanguages(lang).toPromise();
      localStorage.setItem('languages', JSON.stringify(languages));
      const uri = decodeURI(this.uri);
      const check = this.checkLocalRole();
      if (check ) {
        const uri = decodeURI(this.uri);
        this.router.navigate([uri]);
      } else {
        let backUrl = '/dashboard';
        this.router.navigate([backUrl]);
      }

      this.alertifyService.success(this.trans.instant('Login Success!'));
      this.loading = 0;


    } catch (error) {
     if (error.indexOf('error') == -1) {
       this.alertifyService.warning(this.trans.instant(error), true);
     } else {
      this.alertifyService.warning(this.trans.instant('Server error!'), true);

     }
      this.loading = 0;
    }
  }

  async loginRememberMe(key) {


    this.loading = 1;
    if (!key) {
     setTimeout(() => {
      this.loading = 0;
      return;
     }, 500);
    }
    try {
      const data = await this.authService.loginRememberMe(key).toPromise();
      const currentLang = localStorage.getItem('lang');
      if (currentLang) {
        localStorage.setItem('lang', currentLang);
      } else {
        localStorage.setItem('lang', 'en');
      }

      if (this.remember) {
        this.cookieService.set('remember', 'Yes');
        this.cookieService.set('key_temp', data.user.id);
      } else {
        this.cookieService.set('remember', 'No');
        this.cookieService.set('key_temp', '');
      }
      const lang = localStorage.getItem('lang');

      const functions = await this.authService.getPermissions(data.user.id,lang).toPromise();
      localStorage.setItem("functions", JSON.stringify(functions));

      const languages = await this.authService.getLanguages(lang).toPromise();
      localStorage.setItem('languages', JSON.stringify(languages));

      const uri = decodeURI(this.uri);
      const check = this.checkLocalRole();
      if (check ) {
        const uri = decodeURI(this.uri);
        this.router.navigate([uri]);
      } else {
        let backUrl = '/dashboard';
        this.router.navigate([backUrl]);
      }
      this.loading = 0;
    } catch (error) {
      if (error.indexOf('error') == -1) {
        this.alertifyService.warning(this.trans.instant(error), true);
      } else {
       this.alertifyService.warning(this.trans.instant('Server error!'), true);

      }
      this.loading = 0;
    }
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }


  checkLocalRole(): boolean {
    let navs = [];
    const accountType = JSON.parse(localStorage.getItem('user'))?.accountType || '';
    const uri = decodeURI(this.uri);
    if (uri == '/login') {
      return false;
    }
    const permissions = navs.map(x => x.url);
    for (const url of permissions) {
      if (uri.includes(url)) {
        return true;
      }
    }
    return false;
  }
}
