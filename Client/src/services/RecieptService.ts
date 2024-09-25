import { AxiosResponse } from "axios";
import { IReciept } from "../models/Reciept";
import $api from "../http";

export default class RecieptsService {

    static async getReciepts(
        page: number,
        count: number
    ): Promise<AxiosResponse<IReciept[]>> {
        return $api.get<IReciept[]>(`/products/api/Reciepts/all?page=${page}&count=${count}`);
    }

    static async deleteReciept(
        id: string
    ): Promise<AxiosResponse> {
        return $api.delete(`/products/api/Reciepts/${id}`);
    }

    static async updateReciept(
        data: IReciept
    ): Promise<AxiosResponse> {
        return $api.patch(`/products/api/Reciepts/${data.id}`, {
            "name": data.name,
            "cookDuration":data.cookDuration,
            "portions":data.portions,
            "kkal":data.kkal,
        });
    }

    static async addReciept(
        data: IReciept
    ): Promise<AxiosResponse> {
        return $api.put('/products/api/Reciepts', {
            "name": data.name,
            "cookDuration":data.cookDuration,
            "portions":data.portions,
            "kkal":data.kkal,
        });
    }



}