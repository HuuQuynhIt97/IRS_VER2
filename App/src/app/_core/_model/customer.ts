export interface Customer {
  id: number;
  customerNo: string;
  customerName: string;
  customerSex: string;
  customerSexName: string;
  customerBirthday: string | null;
  customerNickname: string;
  customerTel: string;
  customerMobile: string;
  customerAddress: string;
  customerIdcard: string;
  customerEmail: string;
  contactName: string;
  contactTel: string;
  contactMobile: string;
  contactEmail: string;
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
