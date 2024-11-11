import {AxiosResponse} from "axios";
import $api from "../http";
import { IUserLogin } from "../models/User";
export default class UserService {
    static async fetchUserById(id:number):Promise<AxiosResponse> {
        return $api.get(`/api/User/${id}`);
    }

    static async updateUserProfile(
        userId:number,
        user:IUserLogin
    ):Promise<AxiosResponse>{
        return $api.patch(`/api/User/${userId}`, {
            "name": user.Name,
            "login": user.Login,
            "email": user.Email,
            "password": user.Password
        });
    }
}