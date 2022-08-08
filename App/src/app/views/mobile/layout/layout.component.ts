import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { SidebarComponent } from '@syncfusion/ej2-angular-navigations';
import { CookieService } from 'ngx-cookie-service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { AuthService } from 'src/app/_core/_service/auth.service';
import { SiteService } from 'src/app/_core/_service/club-management/site.service';
import { DashboardService } from 'src/app/_core/_service/dashboard.service';
import { Location } from "@angular/common";
@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class LayoutComponent implements OnInit {

  fieldsLang: object = { text: "name", value: "id" };
  farmData: any[];
  farmGuid: any;
  fields: object = { text: "siteName", value: "guid" };
  lang: string;
  languageData = [
    { id: "Tw", name: "Tw" },
    { id: "Cn", name: "Cn" },
    { id: "En", name: "En" },
    { id: "Vi", name: "Vi" },
  ];
  @ViewChild("sidebarTreeviewInstance")
  public sidebarTreeviewInstance: SidebarComponent;
  public width: string = "290px";
  mediaQuery: string = "(min-width: 600px)";
  target: string = ".main-content";
  constructor(
    private router: Router,
    private location: Location,
    private trans: TranslateService,
    private authService: AuthService,
    private cookieService: CookieService,
    private alertify: AlertifyService,
    private serviceDash: DashboardService,
    private service: SiteService,

  ) {}

  public data: Object[] = [
    {
      nodeId: "01",
      nodeText: this.trans.instant("Desktop mode"),
      iconCss: "icon-microchip icon",
    },
    
    {
      nodeId: "10",
      nodeText: "Sign out",
      iconCss: "icon-bookmark-empty icon",
    },
  ];
  public field: Object = {
    dataSource: this.data,
    id: "nodeId",
    text: "nodeText",
    child: "nodeChild",
    iconCss: "iconCss",
  };

  openClick() {
    this.sidebarTreeviewInstance.toggle();
  }
  goToHome() {
    this.router.navigate(["/mobile/home"]);
  }
  ngOnInit() {
    this.farmGuid = localStorage.getItem("farmGuid");
    this.lang = this.capitalize(localStorage.getItem("lang"));
    this.sidebarTreeviewInstance?.hide();
    this.getFarmsByAccount();
  }
  getFarmsByAccount() {
    this.service.getSiteByAccount().subscribe((data: any) => {
      this.farmData = data;
      const farmGuid = localStorage.getItem("farmGuid");
      if (farmGuid) {
        this.farmGuid = farmGuid;
      } else {
        this.farmGuid = data[0]?.guid || "";
      }
      localStorage.setItem("farmGuid", this.farmGuid);
    });
  }
  toggleSidebar() {
    this.router.navigate(["/mobile/home"]);
    // this.sidebarTreeviewInstance.toggle();
  }
  goBack() {
    const homeUrl = this.router.url.includes("home");
    if (!homeUrl) {
      this.location.back();
    }
  }
  onCreated(e: any): void {
    this.sidebarTreeviewInstance.element.style.visibility = "visible";
  }

  logout() {
    this.authService.logOut().subscribe(() => {
      const uri = this.router.url;
      this.cookieService.deleteAll("/");

      this.router.navigate(["/mobile/login"], {
        queryParams: { uri },
        replaceUrl: true,
      });
      this.alertify.message(this.trans.instant("Logged out"));
    });
  }
  onNodeClicked(e) {
    console.log(e);
    if (e.node.dataset.uid === "10") {
      this.logout();
      return;
    } else if (e.node.dataset.uid === "01") {
      this.router.navigate(["/login"]);
      return;
    }
  }
  langValueChange(args) {
    const lang = args.itemData.id.toLowerCase();
    localStorage.removeItem("lang");
    localStorage.setItem("lang", lang);
    this.lang = this.capitalize(localStorage.getItem("lang"));
    location.reload();
  }
  capitalize(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
  }
  farmValueChange(args) {
    this.farmGuid = args.itemData.guid || "";
    localStorage.setItem("farmGuid", args.itemData.guid);
    this.serviceDash.changeFarmGuid(this.farmGuid);

    if (args.isInteracted === true) {
      location.reload();
    }
  }

}
