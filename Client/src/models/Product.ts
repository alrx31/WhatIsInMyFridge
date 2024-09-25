export interface IProduct{
    Id: string;
    Name: string;
    PricePerKilo: number;
    ExpirationTime: string;
    Count:number;
    AddTime: string;
}
export interface IAddProductModel{
    productIdcount:string;
    count:number;
}