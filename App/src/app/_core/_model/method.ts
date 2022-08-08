export interface Method {
  id: number;
  name: string;
  status: boolean | null;
  createBy: number;
  updateBy: number | null;
  deleteBy: number | null;
  createDate: string;
  updateDate: string | null;
  deleteDate: string | null;
}
