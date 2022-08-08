import { SysMenuService } from 'src/app/_core/_service/sys-menu.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { IBreadcrumb } from './breadcrumb.interface';
import { filter, distinctUntilChanged, map } from 'rxjs/operators';
import { NgbTooltipConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.css'],
  providers: [NgbTooltipConfig],
})
export class BreadcrumbComponent implements OnInit {
  isAdmin = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_CANCEL';
  public breadcrumbs: IBreadcrumb[];
  menuItem: any;
  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private serviceSysMenu: SysMenuService,
    config: NgbTooltipConfig
  ) {
    if (this.isAdmin === false) {
      config.disableTooltip = true;
    }
    this.breadcrumbs = this.buildBreadCrumb(this.activatedRoute.root).filter(x=> x.root !== true);
  }

  ngOnInit() {
    this.getMenuItem();
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      distinctUntilChanged(),
    ).pipe(map(() => this.activatedRoute)).subscribe((e) => {
      this.getMenuItem();
      this.breadcrumbs = this.buildBreadCrumb(this.activatedRoute.root).filter(x=> x.root !== true);;
    });
    //this.level = JSON.parse(localStorage.getItem('level')).level as number ;
  }

  buildBreadCrumb(route: ActivatedRoute, url: string = '', breadcrumbs: IBreadcrumb[] = []): IBreadcrumb[] {
    // debugger
    // If no routeConfig is avalailable we are on the root path
    let disable = route.routeConfig && route.routeConfig.data ? route.routeConfig.data.disable : false;
    let root = route.routeConfig && route.routeConfig.data ? route.routeConfig.data.root : false;
    let label = route.routeConfig && route.routeConfig.data ? route.routeConfig.data.breadcrumb : '';
    let module = route.routeConfig && route.routeConfig.data ? route.routeConfig.data.module : '';
    //const isClickable = route.routeConfig && route.routeConfig.data && route.routeConfig.data.isClickable;
    let path = route.routeConfig && route.routeConfig.data ? '' + route.routeConfig.path : '';
    // If the route is dynamic route such as ':id', remove it

    const lastRoutePart = path.split('/').pop();
    const isDynamicRoute = lastRoutePart.startsWith('/');
    if (isDynamicRoute && !!route.snapshot) {
      const paramName = lastRoutePart.split('/')[1];
      path = path.replace(lastRoutePart, route.snapshot.params[paramName]);
      label = route.snapshot.params[paramName];
    }
    // In the routeConfig the complete path is not available,
    // so we rebuild it each time
    // const nextUrl = path;
    const nextUrl = path ? `${url === '' ? '' : url}/${path}` : url;
    const breadcrumb: IBreadcrumb = {
      label,
      url: nextUrl,
      root: root,
      module: module,
      disable
    };
    // Only adding route with non-empty label
    const newBreadcrumbs = breadcrumb.label ? [...breadcrumbs, breadcrumb] : [...breadcrumbs];
    if (route.firstChild) {
      // If we are not on our current path yet,
      // there will be more children to look after, to build our breadcumb
      return this.buildBreadCrumb(route.firstChild, nextUrl, newBreadcrumbs);
    }
    return newBreadcrumbs;
  }
  gotoRouter(data) {
    if (data.url.includes(data.module)) {
      this.router.navigate([data.url]);

    } else {
      const url = data.module ? `/${data.module}${data.url}` : '/';
      this.router.navigate([url]);
    }

  }
    getMenuItem() {
    const url = this.router.url;
    if (url.includes('/report/')) {
      const kind = url;
      this.serviceSysMenu.getItemByKind(localStorage.getItem('lang'),kind ).subscribe( menuItem => {
        this.menuItem = menuItem;
        localStorage.setItem('menuItem', JSON.stringify(this.menuItem ));
      })
    } else {
      this.menuItem = {};
      localStorage.setItem('menuItem', JSON.stringify(this.menuItem));

    }

  }

}
