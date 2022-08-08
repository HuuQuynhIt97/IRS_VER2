import { DatePipe } from '@angular/common';
import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ChartTheme, ILoadedEventArgs } from '@syncfusion/ej2-angular-charts';
import { Browser } from '@syncfusion/ej2-base';
import { Subscription } from 'rxjs';
import { DashboardService } from 'src/app/_core/_service/dashboard.service';
export interface Dashboard {
  title: string;
  data: any;
}
@Component({
  selector: 'app-dashboard-container',
  templateUrl: './dashboard-container.component.html',
  styleUrls: ['./dashboard-container.component.scss'],
  providers: [DatePipe]
})
export class DashboardContainerComponent implements OnInit, OnDestroy {
  @Input() dashboardGuid = '';
  reportType = 'Default';
  dataSource = [];

  //Initializing Primary X Axis
  public primaryXAxis: Object = {
    valueType: 'Category', interval: 1, majorGridLines: { width: 0 }
  };
  //Initializing Primary Y Axis
  public primaryYAxis: Object = {
    majorGridLines: { width: 0 },
    majorTickLines: { width: 0 }, lineStyle: { width: 0 }, labelStyle: { color: 'transparent' }
  };
  public marker: Object = { dataLabel: { visible: true, position: 'Center', font: { fontWeight: '600', color: '#ffffff' } } }
  public title: string = '';
  public tooltip: Object = {
    enable: true
  };
  // custom code start
  public load(args: ILoadedEventArgs): void {
    let selectedTheme: string = location.hash.split('/')[1];
    selectedTheme = selectedTheme ? selectedTheme : 'Material';
    args.chart.theme = <ChartTheme>(selectedTheme.charAt(0).toUpperCase() + selectedTheme.slice(1)).replace(/-dark/i, "Dark");
    if (selectedTheme === 'highcontrast') {
      // args.chart.series[0].marker.dataLabel.font.color= '#000000';
      // args.chart.series[1].marker.dataLabel.font.color= '#000000';
      // args.chart.series[2].marker.dataLabel.font.color= '#000000';
    }
  };
  // custom code end
  public chartArea: Object = {
    border: {
      width: 0
    }
  };
  public width: string = Browser.isDevice ? '100%' : '100%';

  public legendSettings: Object = {
    visible: false
  };
  yearMonth = new Date();
  subscriptions: Subscription[] =  [];
  constructor(private service: DashboardService, private datePipe: DatePipe) { }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x=> x.unsubscribe())
  }
  ngOnInit() {
    this.reportType = 'Default';
    this.subscriptions.push(this.service.currentDashboard.subscribe( guid => {
      this.dashboardGuid = guid;
      if (this.dashboardGuid && typeof(guid) == 'string') {
        this.loadData();
      }
    }));
  }
  onChange(args) {
    if (args.isInteracted) {
      const yearMonth = this.datePipe.transform(args.value, "yyyy-MM-dd");
      this.filter(yearMonth);
    }
  }
  loadData() {
    let lang = localStorage.getItem('lang');
    this.reportType = 'Default';
    this.service.getDashboards(this.dashboardGuid, this.datePipe.transform(this.yearMonth, "yyyy-MM-dd"), lang).subscribe(data => {
      this.dataSource = data;
      if (this.dataSource.length === 0) {
        this.reportType = 'EmptyData';
      } else {
        this.reportType = 'Loaded';
      }
    }, () => {this.reportType = 'Default';});
  }
  back() {
    const yearMonth = this.datePipe.transform(new Date(), "yyyy-MM-dd");
    this.filter(yearMonth);
  }
  filter(yearMonth) {
    this.reportType = 'Default';
    let lang = localStorage.getItem('lang');

    this.service.getDashboards(this.dashboardGuid, yearMonth, lang).subscribe(data => {
      this.dataSource = data;
      this.reportType = 'Loaded';
      if (this.dataSource.length === 0) {
        this.reportType = 'NoData';
      }
    }, () => {this.reportType = 'Default';});
  }
}
