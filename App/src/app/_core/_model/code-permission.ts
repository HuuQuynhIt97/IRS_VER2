export interface CodePermission {
  id: number;
  codeType: string;
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
}