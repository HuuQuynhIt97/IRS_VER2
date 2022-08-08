export interface Site {
    id: number;
    type: string;
    siteNo: string;
    siteName: string;
    siteLocation: string;
    sitePhoto: string;
    file: string;
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

export interface SiteScreen {
    id: number;
    siteName: string;
    guid: string;
}