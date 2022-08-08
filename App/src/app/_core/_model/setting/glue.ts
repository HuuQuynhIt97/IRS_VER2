export interface Glue {
    id: number;
    name: string;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    guid: string;
    unit: string;
}

export interface GlueScreen {
    id: number;
    name: string;
    guid: string;
}