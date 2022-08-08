import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ChartTheme, ILoadedEventArgs } from '@syncfusion/ej2-angular-charts';
import { Browser } from '@syncfusion/ej2-base';
export interface Dashboard {
  title: string;
  data: any;
}
@Component({
  selector: 'app-container1',
  templateUrl: './container1.component.html',
  styleUrls: ['./container1.component.scss']
})
export class Container1Component implements OnInit {
  dashboadData = [
    {
      title: '配種率',
      data: [
        {
          dataSource: [{ x: '總頭數', y: 256 }],
          fill: '#00bfbf',
        },
        {
          dataSource: [{ x: '完成度', y: 252 }],
          fill: '#caf982',
        }
      ]
    },
    {
      title: '分娩率',
      data: [
        {
          dataSource: [{ x: '總頭數', y: 256 }],
          fill: '#00bfbf',
        },
        {
          dataSource: [{ x: '完成度', y: 252 }],
          fill: '#caf982',
        }
      ]
    },
    {
      title: '仔豬死亡率',
      data: [
        {
          dataSource: [{ x: '總頭數', y: 256 }],
          fill: '#00bfbf',
        },
        {
          dataSource: [{ x: '完成度', y: 35 }],
          fill: '#ec808d',
        }
      ]
    },
    {
      title: '仔肉豬育成率',
      data: [
        {
          dataSource: [{ x: '總頭數', y: 256 }],
          fill: '#00bfbf',
        },
        {
          dataSource: [{ x: '完成度', y: 121 }],
          fill: '#f59a23',
        }
      ]
    },
    {
      title: '仔肉豬育成率',
      data: [
        {
          dataSource: [{ x: '總頭數', y: 256 }],
          fill: '#00bfbf',
        },
        {
          dataSource: [{ x: '完成度', y: 121 }],
          fill: '#f59a23',
        }
      ]
    }
  ]
  data2 = [
    {
      a: '新女豬',
      b: 20,
      c: 1.25,
      d: 90,
      e: 7,
      f: 50,
      g: 14,
      h: 7,
      j: 50,
      k: 14,
    },
    {
      a: '母豬',
      b: 19,
      c: 3.55,
      d: 60,
      e: 10,
      f: 50,
      g: 20,
      h: 10,
      j: 50,
      k: 20,
    },
    {
      a: '公豬',
      b: 20,
      c: 5.22,
      d: 50,
      e: 31,
      f: 50,
      g: 62,
      h: 31,
      j: 50,
      k: 62,
    },
    {
      a: '保育豬',
      b: 18,
      c: 2.12,
      d: 70,
      e: 0,
      f: 0,
      g: null,
      h: 12,
      j: 50,
      k: 62,
    },
    {
      a: '生長豬',
      b: 19,
      c: 3.55,
      d: 95,
      e: 12,
      f: 50,
      g: 24,
      h: 12,
      j: 50,
      k: 24,
    },
    {
      a: '肥育豬',
      b: 19,
      c: 3.55,
      d: 80,
      e: 8,
      f: 50,
      g: 16,
      h: 8,
      j: 50,
      k: 16,
    },
    {
      a: '外購仔肉豬',
      b: 20,
      c: 2.12,
      d: 60,
      e: 20,
      f: 50,
      g: 40,
      h: 20,
      j: 50,
      k: 40,
    }
  ]
  data3 = [
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '955',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '999',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '958',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '665',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '1000',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '996',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: 'ABD消毒水',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '992',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '999',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: 'AVG 25KG',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '997',
      d: 'NR01-300KG',
      e: '',
      f: '2 發燒',
      g: '',
      h: 'A042->B055',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '997',
      d: 'NR01-300KG',
      e: 'CDF2',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '997',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },
    {
      a: '劉德華',
      b: '區A.棟D.房A005.欄105262',
      c: '992',
      d: 'NR01-300KG',
      e: '',
      f: '',
      g: '',
      h: '',
      j: '',
      k: '',
    },

  ]

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
  public title: string = 'Olympic Medal Counts - RIO';
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
  constructor() { }

  ngOnInit() {
    const isAdminLang = JSON.parse(localStorage.getItem('user'))?.groupCode === 'ADMIN_LANGUAGE';
    const reloaded = localStorage.getItem('reloaded');
    if (isAdminLang && !reloaded) {
      localStorage.setItem('reloaded', '1');
      location.reload();
    }
  }

}
