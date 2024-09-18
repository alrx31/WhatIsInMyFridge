export interface IFridge {
    id: number;
    name: string
    model: string;
    serial: string | null;
    boughtDate: Date;
    boxNumber: number;
}