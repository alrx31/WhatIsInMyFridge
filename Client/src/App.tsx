import React, {useContext, useEffect, useState} from 'react';
import './App.scss';
import {Route, Routes, useNavigate} from 'react-router-dom'
import Login from "./Components/Register/Login";
import Register from "./Components/Register/Register";
import {Context} from "./index";
import {observer} from "mobx-react-lite";
import {Waiter} from "./Components/Waiter/Waiter";
import {Profile} from "./Components/Profile/Profile";
import { List } from './Components/List/List';
import { FridgePage } from './Components/Fridge/FridgePage';

function App() {

    let history = useNavigate();

    const {store} = useContext(Context)
    
    useEffect(() => {
        try {
            store.checkAuth()
        } catch (e: any) {
            console.error(e);
        }
        finally{
            console.log(store.isAuth);
            if (!store.isAuth) {
                history('/login');  
            }
        }  
    }, []);
    
    
    if (store.isLoading) {
        return <Waiter/>
    }
    

    const logoutHandle = async () => {
        try {
            console.log("startLogout");
            await store.logout();
        } catch (e) {
            console.error(e);
        }
        finally{
            history('/login');
        }
    };

  return (
      
    <div className="App">
        <header>
                <h2>What Is In My Fridge</h2>

                <div className="list-page__buttons">
                    <button className="button__logout" onClick={logoutHandle}>
                        LogOut
                    </button>
                    <button onClick={() => history(`/user/${store.user.Id}`)}>
                        Profile
                    </button>
                </div>
            </header>
      

    
      <Routes>
          <Route path={"/"} element={<List />} />
          <Route path={"/fridges"} element={<List />} />
          

          <Route path={"/login"}  element={<Login/>} />
          <Route path={"/register"} element={<Register />} />
          
          <Route path={"/fridge/:FridgeId"} element={<FridgePage />} />

          <Route path={"/user/:UserId"} element={<Profile />} />
      </Routes>
    </div>
  );
}

export default observer(App);