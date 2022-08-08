export interface Chemical {
    id: number;
    name: string;
    nameEn: string;
    code: string;
    createdDate: Date;
    voc: number;
    supplierID: number;
    allow: number;
    processID: number;
    expiredTime: number;
    createdBy: number;
    modify: boolean;
    daysToExpiration: number;
    guid: string;
    materialNO: string;
    unit: number;
}