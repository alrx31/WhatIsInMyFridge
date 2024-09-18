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

    let [isLoad, setIsLoad] = React.useState(true);

    let history = useNavigate();

    let { store } = useContext(Context);

    let [user, setUser] = useState<IUser>({} as IUser);

    
    useEffect(() => {
        console.log(user); // Это сработает после обновления состояния
    }, [user]);
    
    useEffect(() => {
        if (UserId) {
            getUser(Number(UserId));
        }
    }, [UserId]);

    const getUser = async (UserId: number) => {
        try {
            const response= await UserService.fetchUserById(UserId);
            if (response.status === 200) {

                console.log(response.data);

                const userData: IUser = {
                    Id: response.data.id,
                    Login: response.data.login,
                    Name: response.data.name,
                    Email: response.data.email,
                    IsAdmin: response.data.isAdmin
                };
                setUser(userData);

                console.log(user);
            }
        } catch (err) {
            console.log(err);
        } finally {
            setIsLoad(false);
        }
    };

    if(isLoad){
        return <Waiter />
    }
    
    if (!user?.Name) {
        return <h2>User not Found</h2>
    }

    

    
    
    
    return (
        <div className={"profile-page"}>
            <h2>Профиль пользователя </h2>

            <h3>Name: {user.Name}</h3>
            <h3>Login: {user.Login}</h3>
            <h3>Email: {user.Email}</h3>

            {user.IsAdmin && <h3>Yor are admin</h3>}

            <div className="profile-controlls">
                <button
                    onClick={() => history(`/`)}
                >Назад
                </button>
            </div>
        </div>
    );
}