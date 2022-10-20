import { TranslateService } from '@ngx-translate/core';
import { AlertifyService } from './../../../_core/_service/alertify.service';
import { SysMenuService } from './../../../_core/_service/sys-menu.service';
import { AuthService } from 'src/app/_core/_service/auth.service';
import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { PermissionService } from 'src/app/_core/_service/permission.service';
import { Router } from '@angular/router';
import { TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { DashboardService } from 'src/app/_core/_service/dashboard.service';
import { Subscription } from 'rxjs';
declare let $: any;
@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent implements OnInit,AfterViewInit, OnDestroy {
  menus: any;
  lang: string;
  userid: number;
  title: any;
  btnText: any;
  parentActive = false;
  childActive = false;
  subActive = false;
  farmGuid: any = localStorage.getItem('farmGuid');
  subscription: Subscription = new Subscription();
  sysConf: any;
  constructor(
    private spinner: NgxSpinnerService,
    private sysMenuService: SysMenuService,
    private dashService: DashboardService,
    private translate: TranslateService,
    private alertify: AlertifyService,
    private router: Router,
  ) { }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
  ngAfterViewInit(): void {
    $(function () {
      $('.nav > .sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('.sidebar-toggle').toggleClass('active');
        $('.menu-collapse').toggleClass('active');
        $('.sidebar.slimScroll').toggleClass('active');
      });

      $('.nav > .dropdown .sidebar-toggle').on('click', function (e) {
        e.preventDefault();
        $('.dropdown-menu.dropdown-menu-right.navbar-dropdown').toggleClass('show');
      });

      $('.dropdown-menu-right').on('mouseleave', function (e) {
        e.preventDefault();
        $('.dropdown-menu.dropdown-menu-right.navbar-dropdown').removeClass('show');
      });

    });
  }

  ngOnInit() {
    this.sysConf = JSON.parse(localStorage.getItem('sysConf'))
    this.lang = localStorage.getItem('lang');
    this.userid = +JSON.parse(localStorage.getItem('user'))?.id;
    this.subscription.add(this.dashService.currentFarm.subscribe(farmGuid => {
      this.farmGuid = farmGuid;
      if (this.farmGuid) {
        this.getMenu();
      }

    }))
    if (this.farmGuid) {
      this.getMenu();
    }
    this.loadLang();
  }

  loadLang() {
      this.translate.get("ACCESS_DENIED_MESSAGE").subscribe(res => {
        this.title = res;
      });
      this.translate.get("BACK_TO_LOGIN").subscribe(res => {
        this.btnText = res;
      });
    }
  getMenu() {
    // this.spinner.show();
    this.sysMenuService.getMenusByFarm(this.lang, this.farmGuid).subscribe((menus: any) => {
      this.menus = menus.filter(x => x.children.length > 0);
      localStorage.setItem('menus', JSON.stringify(menus));
      $(function () {
        $('a.toggle').on('click', function (e) {
          e.preventDefault();
          $(this).closest('ul').find('a.toggle.active').not(this).removeClass('active');
          $(this).toggleClass('active');

        });
      });
      setTimeout(() => {
        // this.spinner.hide();
      }, 500)
    }, (err) => {
      // this.spinner.hide();
    });
  }
  checkRole(data) {
    const functionCode = data.functionCode;
    const functions = JSON.parse(localStorage.getItem('functions')) || [];
    const permissions = functions.includes(functionCode);
    return permissions;
  }
  route(data) {
    const functionCode = data.functionCode;
    if (functionCode === 'Report' && data.level === 2) {
      return;
    }
    if (functionCode === 'Report'&& data.level === 3) {
      return this.router.navigate([data.url])
    }
    const functions = JSON.parse(localStorage.getItem('functions')) || [];
    const permissions = functions.includes(functionCode);
    if(permissions) {
      return this.router.navigate([data.url])

    } else {
      this.alertify.errorBackToLogin(this.title, this.btnText, () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        localStorage.removeItem('refresh-token');
        localStorage.removeItem('login-event');
        localStorage.removeItem('functions');
        localStorage.removeItem('menuItem');
        localStorage.removeItem('farmGuid');
        localStorage.removeItem('menus');
        this.router.navigate(['/login']);
      }, true, () => {
        return;
      });
      return;
    }
  }
  isActive(data) {
    //return this.router.url === data.url;
  }
  navigate(data) {
    const functionCode = data.functionCode;
    if (functionCode === 'Report'&& data.level === 2) {
      return;
    }
    if (functionCode === 'Report'&& data.level === 3) {
      return this.router.navigate([data.url])
    }
    const functions = JSON.parse(localStorage.getItem('functions')) || [];
    const permissions = functions.includes(functionCode);
    if(permissions) {
      return this.router.navigate([data.url])
    } else {
      this.alertify.errorBackToLogin(this.title, this.btnText, () => {
        localStorage.removeItem('user');
        localStorage.removeItem('token');
        localStorage.removeItem('refresh-token');
        localStorage.removeItem('login-event');
        localStorage.removeItem('functions');
        localStorage.removeItem('menuItem');
        localStorage.removeItem('farmGuid');
        localStorage.removeItem('menus');
        this.router.navigate(['/login']);
      }, true, () => {
        return;
      });
      return;
    }
  }
}
