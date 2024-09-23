import $api from '../http'
import { AxiosResponse } from "axios";
import {IAuthResponse} from "../models/AuthResponse";
import Store from "../store/store";

export default class AuthService{
    static async login(
        login:string,
        password:string
    ):Promise<AxiosResponse>{
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
        console.log(email,password,name,login)
        return $api.put<IAuthResponse>('/api/Auth', {
            name,
            login,
            email,
            password
        })
    }

    static async logout(userId:number):Promise<AxiosResponse>{
        return $api.delete<IAuthResponse>(`/api/Auth/${userId}`)
    }
}