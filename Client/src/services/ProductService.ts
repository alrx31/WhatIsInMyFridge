import { AxiosResponse } from "axios";
import $api from "../http";

export default class UserService {
    static async getProductsByFridgeId(id: number): Promise<AxiosResponse> {
        return $api.get(`fridge/api/Fridge/${id}/products`);
    }
}