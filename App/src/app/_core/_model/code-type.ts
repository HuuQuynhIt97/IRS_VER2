export interface CodeType {
  id: number;
  codeType1: string;
  codeNo: string;
  codeName: string;
  comment: string;
  status: string;
  createBy: number | null;
  createDate: string | null;
  updateBy: number | null;
  updateDate: string | null;
  sort: number | null;
  codeNameEn: string;
  codeNameCn: string;
  codeNameVn: string;
  storeId: number | null;
  webSiteId: number | null;
}
