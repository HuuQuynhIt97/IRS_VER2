export interface Part {
    id: number;
    name: string;
    partNameCn: string;
    partNameEn: string;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    guid: string;
}

export interface PartScreen {
    id: number;
    guid: string;
}