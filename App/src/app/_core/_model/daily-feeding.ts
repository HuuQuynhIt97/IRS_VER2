export interface DailyFeeding {
  id: number;
  unit: number;
  foodID: number;
  food: any;
  status: boolean | null;
  createBy: number;
  updateBy: number | null;
  deleteBy: number | null;
  createDate: string;
  updateDate: string | null;
  deleteDate: string | null;
}
