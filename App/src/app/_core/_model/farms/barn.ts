export interface Barn {
  id: number;
  farmGuid: string;
  areaGuid: string;
  type: string;
  barnNo: string;
  barnName: string;
  barnPrincipal: string;
  barnLength: number | null;
  barnWidth: number | null;
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
export interface BarnScreen {
  id: number;
  barnName: string;
  guid: string;
}
