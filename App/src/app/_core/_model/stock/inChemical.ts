export interface InChemical {
  id: number;
  chemicalGuid: string;
  supplierGuid: string;
  code: string;
  name: string;
  percentage: string;
  unit: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  deleteDate: string | null;
  deleteBy: number | null;
  status: number | null;
  guid: string;
  qrCode: string;
}
