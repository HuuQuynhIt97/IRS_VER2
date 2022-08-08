import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Browser } from "@syncfusion/ej2-base";
import { Subscription } from "rxjs";
import { DashboardService } from "src/app/_core/_service/dashboard.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "./dashboard.component.html",
  styleUrls: ["./dashboard.component.scss"],
})
export class DashboardComponent implements OnInit, OnDestroy {
  dashboardGuid = null;
  farmGuid = localStorage.getItem("farmGuid");
  guid: any;
  status = 'Fetching';
  subscription: Subscription = new Subscription();
  sysConf: any;
  constructor(
    private service: DashboardService,
    private route: ActivatedRoute,
    private router: Router
  ) {

  }
  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  data: any = [];
  ngOnInit() {
    this.sysConf = JSON.parse(localStorage.getItem('sysConf'))
    this.dashboardGuid = this.route.snapshot.paramMap.get("guid");
    this.subscription.add(
      this.service.currentFarm.subscribe((farmGuid: string) => {
        this.farmGuid = farmGuid;
        this.loadData();
      })
    );
  }
  public width: string = Browser.isDevice ? "100%" : "100%";
  showData(value) {
    this.dashboardGuid = value.guid;
    this.router.navigate(["/dashboard/" + this.dashboardGuid]);
    this.service.changeDashboardGuid(this.dashboardGuid);
  }

  loadData() {
    this.status = 'Fetching';
    this.service.getDashboardsNav(this.farmGuid).subscribe((data) => {
      this.data = data;
      this.dashboardGuid = this.data[0]?.guid || "";
      this.service.changeDashboardGuid(this.dashboardGuid);
      this.router.navigate(["/dashboard/" + this.dashboardGuid]);
      this.status = 'Loaded';

    });
  }
}
