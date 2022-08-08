export interface Room {
  id: number;
  farmGuid: string;
  areaGuid: string;
  barnGuid: string;
  type: string;
  roomNo: string;
  roomName: string;
  file: string;
  roomPrincipal: string;
  roomLength: number | null;
  roomWidth: number | null;
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
  roomPhoto1: string;
  roomPhoto2: string;
  roomPhoto3: string;
  roomPhoto4: string;
  roomPhoto5: string;
}
export interface RoomScreen {
  id: number;
  roomName: string;
  guid: string;
}
