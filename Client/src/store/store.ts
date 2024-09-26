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
            if (response.data.user) this.setUser({
                Id: response.data.user.id,
                Name: response.data.user.name,
                Login: response.data.user.login,
                Email: response.data.user.email,
                IsAdmin: response.data.user.isAdmin
            });
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
        } catch (e: any) {
            console.log(e.response?.data?.message);
        }
        finally{
            localStorage.removeItem('token');
            this.setAuth(false);
            this.setUser({} as IUser);
            console.log("logout");
        }
    }


    async checkAuth() {
        this.setLoading(true);
        try {
            const response = await axios.post(`${API_URL}/api/Auth/token`, {
                "jwtToken": localStorage.getItem('token'),
            }, { withCredentials: true })
            if (response.status !== 200 || !response.data.isLoggedIn) {
                console.log('Ошибка авторизации');
                localStorage.removeItem('token');
                this.setAuth(false);
                this.setUser({} as IUser);
                return;
            };
            localStorage.setItem('token', response.data.jwtToken);
            this.setAuth(true);
            if (response.data.user) this.setUser({
                Id: response.data.user.id,
                Name: response.data.user.name,
                Login: response.data.user.login,
                Email: response.data.user.email,
                IsAdmin: response.data.user.isAdmin
            });
            else console.log('Ошибка получения данных пользователя');

        } catch (e: any) {
            localStorage.removeItem('token');
            this.setAuth(false);
            this.setUser({} as IUser);
            console.log('Ошибка авторизации');
        } finally {
            this.setLoading(false);
        }
    }
}