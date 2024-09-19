import { Axios, AxiosResponse } from "axios";
import $api from "../http";

export default class FridgeService{
    static async getFridgesByUserId(
        userId:number
    ):Promise<AxiosResponse>{
        return $api.get(`/fridge/api/fridge/?userId=${userId}`)
    }

    static async getFridgeById(
        fridgeId:number
    ):Promise<AxiosResponse>{
        return $api.get(`/fridge/api/fridge/${fridgeId}`)
    }
}