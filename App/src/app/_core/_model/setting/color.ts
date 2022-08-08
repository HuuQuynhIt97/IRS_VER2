export interface Color {
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
}

export interface ColorScreen {
    id: number;
    guid: string;
}