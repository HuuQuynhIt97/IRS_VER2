export interface Shoe {
    id: number;
    modelName: string;
    modelNo: string;
    article1: string;
    article2: string;
    remark: string;
    productionDate: string | null;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    version: string | null;
    guid: string;
}

export interface ShoeScreen {
    id: number;
    guid: string;
}