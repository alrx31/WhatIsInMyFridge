import { AxiosResponse } from "axios";
import $api from "../http";
import { IProduct } from "../models/Product";

export default class ProductsService {
    
    static async addProduct(
        data:IProduct
    ):Promise<AxiosResponse>{
        return $api.put('/products/api/Products', {
            "name": data.Name,
            "pricePerKilo": data.PricePerKilo,
            "expirationTime":data.ExpirationTime
        });
    }

    static async getProducts(
        page:number,
        count:number
    ):Promise<AxiosResponse<IProduct[]>>{
        return $api.get<IProduct[]>(`/products/api/Products/all?page=${page}&count=${count}`);
    }

    static async deleteProduct(
        id:string
    ):Promise<AxiosResponse>{
        return $api.delete(`/products/api/Products/${id}`);
    }

    static async updateProduct(
        data:IProduct
    ):Promise<AxiosResponse>{
        return $api.patch(`/products/api/Products/${data.Id}`, {
            "name": data.Name,
            "pricePerKilo": data.PricePerKilo,
            "expirationTime":data.ExpirationTime
        });
    }
}