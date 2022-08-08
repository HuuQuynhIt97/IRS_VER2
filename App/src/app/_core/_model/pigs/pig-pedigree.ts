export interface PigPedigree {
  id: number;
  fatherGuid: string;
  motherGuid: string;
  birthDay: string | null;
  pedigreeName: string;
  fromPigFarm: string;
  breed: string;
  earNo: string;
  earTag: string;
  rfid: string;
  comment: string;
  cancelFlag: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  status: number | null;
  guid: string;
  farmGuid: string;
}
