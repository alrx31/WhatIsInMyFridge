import {IUser} from "../models/User";
import {makeAutoObservable} from "mobx";
import AuthService from "../services/AuthService";
import UserService from "../services/UserService";
import axios from "axios";
import {API_URL} from "../http";
import {IAuthResponse} from "../models/AuthResponse";

export default class Store {
    user = {} as IUser;
    isAuth = false;
    isLoading = false;
    pageSize = 5;

    constructor() {
        makeAutoObservable(this);
    }

    setAuth(bool: boolean) {
        this.isAuth = bool;
    }

    setUser(user: IUser) {
        this.user = user;
    }


    setLoading(bool: boolean) {
        this.isLoading = bool;
    }


    async login(login: string, password: string) {
        this.setLoading(true)
        
        try {
            const response = await AuthService.login(login, password);
            localStorage.setItem('token', response.data.jwtToken);
            this.setAuth(true);
            if (response.data) this.setUser(response.data.user);
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
            const response = await axios.post<IAuthResponse>(`${API_URL}/api/Auth/token`, {
                "jwtToken": localStorage.getItem('token'),
            }, { withCredentials: true })
            localStorage.setItem('token', response.data.jwtToken);
            if (response.data.user == null) throw 'Ошибка получения данных пользователя';
            this.setAuth(true);
            if (response.data.user) this.setUser({
                Id: response.data.user.Id,
                Name: response.data.user.Name,
                Login: response.data.user.Login,
                Email: response.data.user.Email,
                IsAdmin: response.data.user.IsAdmin
            });
            else console.log('Ошибка получения данных пользователя');

        } catch (e: any) {
            console.log(e);
        } finally {
            this.setLoading(false);
        }
    }
}