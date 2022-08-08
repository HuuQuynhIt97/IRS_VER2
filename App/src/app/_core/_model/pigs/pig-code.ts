export interface PigCode {
  id: number;
  earNo: string;
  earTag: string;
  rfid: string;
  pedigreeGuid: string;
  comment: string;
  cancelFlag: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  status: number | null;
  guid: string;
}
