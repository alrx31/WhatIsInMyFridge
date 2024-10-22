import {IUser} from "./User";

export interface IAuthResponse {
    isLoggedIn: boolean;
    user: IUser;
    jwtToken:string;
    refreshToken:string;
}