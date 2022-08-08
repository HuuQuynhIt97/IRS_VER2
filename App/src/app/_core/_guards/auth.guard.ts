import { TranslateService } from '@ngx-translate/core';
import { AuthService } from 'src/app/_core/_service/auth.service';
import { filter } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRoute, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { ActionConstant } from '../_constants';
import { AlertifyService } from '../_service/alertify.service';
import { CookieService } from 'ngx-cookie-service';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  btnText: any;
  title: any;
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private cookieService: CookieService,
    private translate: TranslateService,
    private router: Router,
    private route: ActivatedRoute) {
    this.loadLang();
  }

  loadLang() {
    this.translate.get("Access-denied").subscribe(res => {
      this.title = res;
    });
    this.translate.get("Back to login").subscribe(res => {
      this.btnText = res;
    });
  }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.loggedIn()) {
      if (this.checkRole(route) === false) {
        this.authService.logOut().subscribe(x => {
          localStorage.removeItem('user');
          localStorage.removeItem('token');
          localStorage.removeItem('refresh-token');
          localStorage.removeItem('login-event');
          localStorage.removeItem('functions');
          localStorage.removeItem('menuItem');
          localStorage.removeItem('farmGuid');
          localStorage.removeItem('menus');
          this.cookieService.deleteAll('/');
          this.alertify.errorBackToLogin(this.title, this.btnText, () => {
            this.router.navigate(['/login']);
          });
        });

        return false;
      }
      return true;
    }
    this.router.navigate(['login'], { queryParams: { uri: state.url }, replaceUrl: true });
    return false;
  }
  checkRole(route: ActivatedRouteSnapshot): boolean {
    const functionCode = route.data.functionCode;
    if (functionCode === 'dashboard') {
      return true;
    }
    if (functionCode === 'Change Password') {
      return true;
    }
    if (functionCode === 'Profile') {
      return true;
    }
    if (functionCode) {
      const functions = JSON.parse(localStorage.getItem('functions')) || [];
      const permissions = functions.includes(functionCode);
      return permissions;
    }
    return true;
  }
}

