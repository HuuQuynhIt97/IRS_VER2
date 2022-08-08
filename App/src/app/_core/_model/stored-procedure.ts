export interface StoredProcedure {
  id: number;
  storedName: string;
  storedType: string;
  color: string;
  systemMenuGuid: string;
  comment: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  status: number | null;
  guid: string;
  legend: string;
}
