export interface Ink {
    id: number;
    name: string;
    nameEn: string;
    code: string;
    createdDate: Date;
    voc: number;
    supplierID: number;
    allow: number;
    processID: number;
    expiredTime: number | null;
    createdBy: number;
    daysToExpiration: number | null;
    materialNO: string;
    guid: string;
    unit: number;
}