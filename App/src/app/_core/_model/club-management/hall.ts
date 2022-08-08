export interface Hall {
    id: number;
    type: string;
    siteGuid: string;
    hallNo: string;
    hallName: string;
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

export interface HallScreen {
    id: number;
    hallName: string;
    guid: string;
}