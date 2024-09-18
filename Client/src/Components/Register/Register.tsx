import react, {useContext, useState} from 'react';
import "./Register.scss"
import Store from "../../store/store";
import {Context} from "../../index";
import {NavLink, useNavigate} from "react-router-dom";
import {observer} from "mobx-react-lite";
import React from "react";
const Register = (
    
) => {
    
    const [email,setEmale] = useState("");
    const [login,setLogin] = useState("");
    const [password, setPassword] = useState("")
    const [repPassword, setRepPassword] = useState("")
    const [name,setName] = useState("");

    const [message,setMessage] = useState("");
        
    const {store} = useContext(Context)
    
    const history = useNavigate();
    
    let handleSubmit = (e:any)=>{
        e.preventDefault();

        if(!validateForm()){
            return;
        }


        store.registration(email,password,name,login);

        if (!store.isAuth) {
            history('/login')
        }
    }

    let validateForm = ():boolean=>{
        if(password !== repPassword){
            setMessage("Passwords do not match");
            return false;
        }

        if(password.length < 6){
            setMessage("Password must be at least 6 characters long");
            return false;
        }

        if(!email.includes('@')){
            setMessage("Email is incorrect");
            return false;
        }

        if(name.length < 3){
            setMessage("Name must be at least 3 characters long");
            return false;
        }

        if(login.length < 3){
            setMessage("Login must be at least 3 characters long");
            return false;
        }

        return true;
    }

    
    return (
        <div className="register-page">
            <form onSubmit={handleSubmit} className={"register-form"}>
                <h2>Register</h2>
                <h3>{message}</h3>
                <div className="form-group">
                    <label htmlFor="email">Email</label>
                    <input
                        type="text"
                        id="email"
                        name="email"
                        onChange={e=>setEmale(e.target.value)}
                        value={email}
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
                <div className="form-group">
                    <label htmlFor="password">Repeat Password</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        onChange={e=>setRepPassword(e.target.value)}
                        value={repPassword}

                    />
                </div>
                <div className="form-group">
                    <label htmlFor="firstName">Name</label>
                    <input
                        type="text"
                        id="firstName"
                        name="firstName"
                        onChange={e=>setName(e.target.value)}
                        value={name}
                    />
                </div>
                <div className="form-group">
                    <label htmlFor="lastName">Login</label>
                    <input
                        type="text"
                        id="lastName"
                        name="lastName"
                        onChange={e=>setLogin(e.target.value)}
                        value={login}
                    />
                </div>
                <button type="submit" className="login-button">Register</button>
                <NavLink to={"/login"}>have account?</NavLink>
            </form>
        </div>
    )
}

export default observer(Register);