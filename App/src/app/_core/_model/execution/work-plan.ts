export interface WorkPlan {
    id: number;
    line: string;
    poNo: string;
    modelName: string;
    modelNo: string;
    articleNo: string;
    qty: string;
    treatment: string | null;
    stitching: string | null;
    stockfitting: string | null;
    createBy: number | null;
    scheduleId: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    guid: string;
}

