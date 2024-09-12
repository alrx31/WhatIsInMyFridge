import {IUser} from "../models/User";
import {makeAutoObservable} from "mobx";
import AuthService from "../services/AuthService";
import UserService from "../services/UserService";
import axios from "axios";
import {API_URL} from "../http";
import {IAuthResponse} from "../models/AuthResponse";

export default class Store {
    user = {} as IUser;
    isAuht = false;
    isLoading = false;
    pageSize = 5;

    constructor() {
        makeAutoObservable(this);
    }

    setAuth(bool: boolean) {
        this.isAuht = bool;
    }

    setUser(user: IUser) {
        this.user = user;
    }


    setLoading(bool: boolean) {
        this.isLoading = bool;
    }


    async login(email: string, password: string) {
        this.setLoading(true)
        
        try {
            const response = await AuthService.login(email, password);
            localStorage.setItem('token', response.data.jwtToken);
            this.setAuth(true);
            if (response.data.userId == 0) {
                throw 'Ошибка получения данных пользователя';
            }
            const res = await UserService.fetchUserById(response.data.userId);
            if (res.data) this.setUser(res.data);
            else console.log('Ошибка получения данных пользователя');

        } catch (e: any) {
            console.log(e.response?.data?.message);
        } finally {
            this.setLoading(false);
        }
    }

    async registration(
        email: string,
        password: string,
        name: string,
        login: string,
    ) {
        try {
            const response = await AuthService.register(
                email,
                password,
                name,
                login
            );
            if (response.status === 200) {
                alert('Успешная регистрация');
            }
        } catch (e: any) {
            console.log(e.response?.data?.message);
        }
    }

    async logout() {
        try {
            const response = await AuthService.logout(this.user.Id);
            localStorage.removeItem('token');
            this.setAuth(false);
            this.setUser({} as IUser);
        } catch (e: any) {
            console.log(e.response?.data?.message);
        }
    }


    async checkAuth() {
        this.setLoading(true);
        try {
            const response = await axios.post<IAuthResponse>(`${API_URL}/Participants/refresh-token`, {
                JwtToken: localStorage.getItem('token'),
                RefreshToken: ""
            }, {withCredentials: true})

            localStorage.setItem('token', response.data.jwtToken);
            if (response.data.userId === 0) throw 'Ошибка получения данных пользователя';
            this.setAuth(true);
            
            const res = await UserService.fetchUserById(response.data.userId);
            if (res.data) this.setUser({
                Id: res.data.id,
                Name: res.data.firstName,
                Login: res.data.login,
                Email: res.data.email,
                IsAdmin: res.data.isAdmin
            });
            else console.log('Ошибка получения данных пользователя');

        } catch (e: any) {
            console.log(e?.response?.data?.message);
        } finally {
            this.setLoading(false);
        }
    }
}