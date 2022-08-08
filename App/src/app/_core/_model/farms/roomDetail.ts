export interface RoomDetail {
  hallName: string;
  hallNo: string;
  id: number | 0;
  siteGuid: string;
  hallGuid: string;
  type: string;
  roomNo: string;
  roomName: string;
  roomLocation: string;
  roomSize: number | null;
  roomPhoto: string;
  file: string;
  roomDescript: string;
  price: number | null;
  priceMember: number | null;
  priceSpecial: number | null;
  comment: string;
  createDate: string | null;
  createBy: number | null;
  updateDate: string | null;
  updateBy: number | null;
  deleteDate: string | null;
  deleteBy: number | null;
  status: number | null;
  guid: string;
  roomPhoto1: string;
  roomPhoto2: string;
  roomPhoto3: string;
  roomPhoto4: string;
  roomPhoto5: string;
  roomGallery: File[];

}
export interface RoomScreen {
  id: number;
  roomName: string;
  guid: string;
}
