import React, { useContext, useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Waiter } from '../Waiter/Waiter';
import { Context } from '../../index';
import UserService from '../../services/UserService';
import { IUser } from '../../models/User';
import './Profile.scss';

interface IProfileProps { }

export const Profile: React.FC<IProfileProps> = () => {
    let { UserId } = useParams();
    const [isLoad, setIsLoad] = useState(true);
    const history = useNavigate();
    const { store } = useContext(Context);
    const [user, setUser] = useState<IUser>({} as IUser);

    useEffect(() => {
        if (UserId) {
            getUser(Number(UserId));
        }
    }, [UserId]);

    const getUser = async (UserId: number) => {
        try {
            const response = await UserService.fetchUserById(UserId);
            if (response.status === 200) {
                const userData: IUser = {
                    Id: response.data.id,
                    Login: response.data.login,
                    Name: response.data.name,
                    Email: response.data.email,
                    IsAdmin: response.data.isAdmin,
                };
                setUser(userData);
            }
        } catch (err) {
            console.error(err);
        } finally {
            setIsLoad(false);
        }
    };

    if (isLoad) {
        return <Waiter />;
    }

    if (!user?.Name) {
        return <h2>User not Found</h2>;
    }

    return (
        <div className="profile-page">
            <div className="profile-header">
                <h2>Профиль пользователя</h2>
            </div>
            <div className="profile-info">
                <h3>Name: {user.Name}</h3>
                <h3>Login: {user.Login}</h3>
                <h3>Email: {user.Email}</h3>
                {user.IsAdmin && <h3 className="admin-indicator">You are an admin</h3>}
            </div>
            <div className="profile-controls">
                <button onClick={() => history(`/`)}>Назад</button>
            </div>
        </div>
    );
};
