export interface CullingTank {
  id: number;
  farmGuid: string;
  areaGuid: string;
  type: string;
  cullingTankNo: string;
  cullingTankName: string;
  cullingTankPrincipal: string;
  cullingTankLength: number | null;
  cullingTankWidth: number | null;
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
export interface CullingTankScreen {
  id: number;
  cullingTankName: string;
  guid: string;
}
