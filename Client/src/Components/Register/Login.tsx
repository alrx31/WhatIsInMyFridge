import React, {FormEventHandler, useContext, useEffect, useState} from 'react'
import './Register.scss'
import {NavLink, useNavigate} from "react-router-dom";
import {Waiter} from "../Waiter/Waiter";
import {IUser} from "../../models/User";
import {Context} from "../../index";
import {observer} from "mobx-react-lite";

const Login = (
    
)=>{
    const [login,setLogin] = useState("");
    const [password, setPassword] = useState("")
    const [isLoad,setIsLoad] = useState(false)
    const {store} = useContext(Context);
    const history = useNavigate();

    const [message,setMessage] = useState("");


    let handleSubmit = async  (e:any)=>{
        setIsLoad(true);
        e.preventDefault();
        try {
            await store.login(login, password)   
        } catch (e) {
            console.log("Ошибка входа", e);
        }
        finally{
            console.log(store.isAuth, store.isLoading, store.user?.Id);
            if (store.isAuth && !store.isLoading && store.user?.Id) {
                history('/');
            }else{
                setMessage("Login or password is incorrect");
            }
            setIsLoad(false);
        }
    }

    return (
        <div className="register-page">
            {isLoad ? <Waiter/> : ""}
            <form onSubmit={handleSubmit} className={"login-form"}>
                <h2>Login</h2>
                <h3>{message}</h3>
                <div className="form-group">
                    <label htmlFor="login">Login</label>
                    <input
                        type="text"
                        id="login"
                        name="login"
                        onChange={e=>setLogin(e.target.value)}
                        value={login}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="password">Password</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        onChange={e=>setPassword(e.target.value)}
                        value={password}
                    />
                </div>
                <button type="submit" className="login-button">Login</button>
                <NavLink to={'/register'}>create account</NavLink>
            </form>
        </div>
    )
}

export default observer(Login)