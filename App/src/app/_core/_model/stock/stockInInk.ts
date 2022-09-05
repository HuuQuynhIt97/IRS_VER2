export interface StockInInk {
  id: number;
  inkGuid: string;
  realAmount: number | null;
  remainingAmount: number | null;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  deleteDate: string | null;
  deleteBy: number | null;
  executeDate: string | null;
  approveDate: string | null;
  approveBy: number | null;
  status: number | null;
  guid: string;
}
