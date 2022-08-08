export interface WorkOrder {
    id: number;
    hallGuid: string;
    workOrderNo: string;
    workOrderName: string;
    comment: string;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    guid: string;
}