export interface Farm {
  id: number;
  type: string;
  farmNo: string;
  farmName: string;
  farmPrincipal: string;
  farmLength: number | null;
  farmWidth: number | null;
  farmTel: string;
  farmAddress: string;
  longitude: string;
  latitude: string;
  temptureTopLimit: string;
  temptureLowLimit: string;
  farmGgp: number | null;
  farmGp: number | null;
  farmPmpf: number | null;
  farmSemen: number | null;
  farmNursery: number | null;
  farmGrower: number | null;
  comment: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  deleteDate: string | null;
  deleteBy: number | null;
  status: number | null;
  guid: string;
  cancelFlag: string;
  startDate: any | null;
  endDate: any | null;
}
export interface FarmScreen {
  id: number;
  farmName: string;
  guid: string;

}
