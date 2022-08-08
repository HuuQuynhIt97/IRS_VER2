export interface SysMenu {
  id: number;
  type: string;
  menuName: string;
  menuNameCn: string;
  menuNameEn: string;
  menuNameVn: string;
  comment: string;
  menuIcon: string;
  menuLink: string;
  sortId: number;
  upperId: number;
  storedProceduresName: string;
  reportType: string;
  status: any;

  chartName: string;
  chartNameCn: string;
  chartNameEn: string;
  chartNameVn: string;
  chartUnit: string;

  chartXAxisName: string;
  chartXAxisNameCn: string;
  chartXAxisNameEn: string;
  chartXAxisNameVn: string;

  chartYAxisName: string;
  chartYAxisNameCn: string;
  chartYAxisNameEn: string;
  chartYAxisNameVn: string;

  farmGgp: number | null;
  farmGp: number | null;
  farmPmpf: number | null;
  farmSemen: number | null;
  farmNursery: number | null;
  farmGrower: number | null;
}
