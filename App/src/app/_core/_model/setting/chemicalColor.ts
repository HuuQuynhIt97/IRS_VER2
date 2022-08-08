export interface ChemicalColor {
    id: number;
    chemicalGuid: string;
    colorGuid: string;
    createDate: string | null;
    createBy: number | null;
    updateDate: string | null;
    updateBy: number | null;
    deleteDate: string | null;
    deleteBy: number | null;
    status: number | null;
    guid: string;
    unit: string;
    percentage: string;
}
