export interface Schedule {
    id: number;
    colorGuid: string;
    shoesGuid: string;
    treatmentWayGuid: string;
    treatmentGuid: string;
    processGuid: string;
    partGuid: string;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    consumption: number | null;
    guid: string;
}

