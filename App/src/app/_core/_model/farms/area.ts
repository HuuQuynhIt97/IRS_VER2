export interface Area {
  id: number;
  farmGuid: string;
  type: string;
  areaNo: string;
  areaName: string;
  areaPrincipal: string;
  areaLength: number | null;
  areaWidth: number | null;
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
export interface AreaScreen {
  id: number;
  areaName: string;
  guid: string;
}
