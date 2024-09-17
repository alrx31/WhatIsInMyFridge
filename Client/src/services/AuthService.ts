import $api from '../http'
import { AxiosResponse } from "axios";
import {IAuthResponse} from "../models/AuthResponse";
import Store from "../store/store";

export default class AuthService{
    static async login(
        login:string,
        password:string
    ):Promise<AxiosResponse<IAuthResponse>>{
        return $api.post<IAuthResponse>('/api/Auth', {
            "Login":login,
            "password":password
        })
    }
    static async register(
        email:string,
        password:string,
        name:string,
        login:string,
    ):Promise<AxiosResponse<IAuthResponse>>{
        return $api.put<IAuthResponse>('/api/Auth', {
            name,
            login,
            email,
            password
        })
    }

    static async logout(userId:number):Promise<AxiosResponse<IAuthResponse>>{
            let ask = {
                "Token":localStorage.getItem('token') ? localStorage.getItem('token') : "",
                "UserId":userId ? userId : 0
            }
        return $api.post<IAuthResponse>('/Participants/logout', ask)
    }
}