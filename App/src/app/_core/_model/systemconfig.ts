export interface SystemConfig {
  id: number;
  type: string;
  value: string;
  no: string;
  sort: number;
  webBuildingId: number | null;
  comment: string;
  accountId: number | null;
  status: number | null;
  createBy: number | null;
  updateBy: number | null;
  deleteBy: number | null;
  createDate: string | null;
  updateDate: string | null;
  deleteDate: string | null;
}
