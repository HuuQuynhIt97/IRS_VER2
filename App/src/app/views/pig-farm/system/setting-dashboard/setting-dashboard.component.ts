import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-setting-dashboard',
  templateUrl: './setting-dashboard.component.html',
  styleUrls: ['./setting-dashboard.component.scss']
})
export class SettingDashboardComponent implements OnInit {
  active = 'Table';
  dashboardGuid = '';
  areaGuid = '';
  constructor() { }

  ngOnInit() {
  }
  onDashboardGuid(value) {
    this.dashboardGuid = value;
  }
  onAreaGuid(value) {
    this.areaGuid = value;
  }
}
