import React, {useContext, useEffect, useState} from 'react';
import {useNavigate, useParams} from "react-router-dom";
import {Waiter} from "../Waiter/Waiter";
import {Context} from "../../index";
import UserService from "../../services/UserService";
import {IUser} from "../../models/User";
import "./Profile.scss";
interface IProfileProps {
}
export const Profile:React.FC<IProfileProps> = ({

})=> {
    let {UserId} = useParams();

    let [isLoad, setIsLoad] = React.useState(false);
    let history = useNavigate();
    let {store} = useContext(Context);
    let [user,setUser] = useState({} as IUser);
    
    
    useEffect(() => {
        setIsLoad(true)
        getUser(Number(UserId));
    }, []);
    
    if(isLoad){
        return <Waiter />
    }
    
    let getUser = (UserId:number)=>{
        try{
            UserService.fetchUserById(UserId)
                .then((response)=>{
                    if(response.status === 200){
                        setUser(response.data);
                    }
                }).catch((err)=>{
                console.log(err);
            })
                .finally(()=>{
                    setIsLoad(false);
                })
        }catch(err){
            console.log(err);
        }
    }
    
    
    return (
        <div className={"profile-page"}>
            {isLoad && <Waiter />}
                <h2>Профиль пользователя </h2>


            
            <div className="profile-controlls">
                <button
                    onClick={() => history(`/`)}
                >Назад
                </button>
            </div>
        </div>
    );
}

