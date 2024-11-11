import { IProduct } from "./Product";

export interface IReciept {
    id:string;
    name:string;
    cookDuration:string;
    portions:number;
    kkal:number;
    products:IProduct[];
}
