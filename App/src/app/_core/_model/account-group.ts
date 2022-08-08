export interface AccountGroup {
  id: number;
  zoneID: number;
  buildingID: number;
  groupNO: string;
  groupName: string;
  status: boolean | null;
  createBy: number;
  updateBy: number | null;
  deleteBy: number | null;
  createDate: string;
  updateDate: string | null;
  deleteDate: string | null;
}
