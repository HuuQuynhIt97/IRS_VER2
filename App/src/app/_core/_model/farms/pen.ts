export interface Pen {
  id: number;
  farmGuid: string;
  areaGuid: string;
  barnGuid: string;
  roomGuid: string;
  type: string;
  penNo: string;
  penName: string;
  penPrincipal: string;
  penLength: string;
  penWidth: number | null;
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
export interface PenScreen {
  id: number;
  penName: string;
  guid: string;
}
