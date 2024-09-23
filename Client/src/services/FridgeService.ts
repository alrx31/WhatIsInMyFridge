import { Axios, AxiosResponse } from "axios";
import $api from "../http";
import { IFridge } from "../models/Fridge";

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

    static async addFridgeToUser(
        userId:number,
        serial:string
    ):Promise<AxiosResponse>{
        return $api.put(`/fridge/api/fridge/${serial}/users/${userId}`)
    }

    static async deleteFridgeFromUser(
        userId:number,
        fridgeId:number
    ):Promise<AxiosResponse>{
        return $api.delete(`/fridge/api/fridge/${fridgeId}/users/${userId}`)
    }

    static async AddFridge(
        fridge:IFridge
    ):Promise<AxiosResponse>{
        console.log(fridge)
        return $api.put(`/fridge/api/fridge`,
            {
            "name": fridge.Name,
            "model": fridge.Model,
            "serial": fridge.Serial,
            "boughtDate": fridge.BoughtDate,
            "boxNumber": fridge.BoxNumber
          })
    }
}