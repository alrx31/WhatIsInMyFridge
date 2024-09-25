import { AxiosResponse } from "axios";
import $api from "../http";

export default class ListService {
    // TODO: static async getListByFridgeId

    static async getProductsByListId
    (
        id: string
    ): Promise<AxiosResponse> {
        return $api.get(`products/api/List/${id}/products`);
    }
}