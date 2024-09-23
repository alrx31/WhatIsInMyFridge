import React, { useContext, useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Waiter } from '../Waiter/Waiter';
import { Context } from '../../index';
import UserService from '../../services/UserService';
import { IUser, IUserLogin } from '../../models/User';
import './Profile.scss';

interface IProfileProps {}

export const Profile: React.FC<IProfileProps> = () => {
    let { UserId } = useParams();
    const [isLoad, setIsLoad] = useState(true);
    const history = useNavigate();
    const { store } = useContext(Context);
    const [user, setUser] = useState<IUser>({} as IUser);

    const [errorMessage, setErrorMessage] = useState('');

    const [isEditing, setIsEditing] = useState(false);
    const [editedUser, setEditedUser] = useState<IUserLogin>({} as IUserLogin);
    const [RPassword, setRPassword] = useState('');

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
                setEditedUser({
                    Id: userData.Id,
                    Login: userData.Login,
                    Name: userData.Name,
                    Email: userData.Email,
                    Password: '',
                } as IUserLogin);

            }
        } catch (err) {
            console.error(err);
        } finally {
            setIsLoad(false);
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.name === 'RPassword') {
            setRPassword(e.target.value);
            return;
        }
        setEditedUser({
            ...editedUser,
            [e.target.name]: e.target.value,
        });
    };

    const handleSave = async () => {
        if (!verifyData()) return;
        try {
            const response = await UserService.updateUserProfile(store.user.Id,editedUser);
            if (response.status === 200) {
                setUser(editedUser); 
                setIsEditing(false); 
            }
        } catch (err) {
            console.error('Error updating user:', err);
            setErrorMessage('Error updating')
        }
    };

    let verifyData  = ()=>{
        if (editedUser.Name.length < 3 ) {
            setErrorMessage("Name must be at least 3 characters long");
            return false;
        }
        if (editedUser.Email.length < 3 || !editedUser.Email.includes('@')) {
            setErrorMessage("Email must be at least 3 characters long and contain @");
            return false;
        }
        if (editedUser.Password.length < 6) {
            setErrorMessage("Password must be at least 6 characters long");
            return false;
        }
        if (editedUser.Password !== RPassword) {
            setErrorMessage("Passwords do not match");
            return false;
        }


        return true;
    }

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
                {isEditing ? (
                    <>
                        {errorMessage && <h3 className="error-message">{errorMessage}</h3>}
                        <div className="editable-field">
                            <h3>Имя:</h3>
                            <input
                                type="text"
                                name="Name"
                                value={editedUser.Name}
                                onChange={handleInputChange}
                            />
                        </div>
                        <div className="editable-field">
                            <h3>Email:</h3>
                            <input
                                type="email"
                                name="Email"
                                value={editedUser.Email}
                                onChange={handleInputChange}
                            />
                        </div>
                        <div className="editable-field">
                            <h3>Password:</h3>
                            <input
                                type="password"
                                name="Password"
                                value={editedUser.Password}
                                onChange={handleInputChange}
                            />
                        </div>
                        <div className="editable-field">
                            <h3>Repeat Password:</h3>
                            <input
                                type="password"
                                name="RPassword"
                                value={RPassword}
                                onChange={handleInputChange}
                            />
                        </div>
                    </>
                ) : (
                    <>
                        <h3>Имя: {user.Name}</h3>
                        <h3>Логин: {user.Login}</h3>
                        <h3>Email: {user.Email}</h3>
                    </>
                )}
                {user.IsAdmin && <h3 className="admin-indicator">Вы администратор</h3>}
            </div>

            <div className="profile-controls">
                {Number(UserId) === store.user.Id ? (
                    <>  
                        {isEditing ? (
                        <button className="save-btn" onClick={handleSave}>Сохранить</button>
                    ) : (
                        <button onClick={() => setIsEditing(true)}>Редактировать</button>
                    )}
                    </>
                ): null}
                <button onClick={() => history(`/`)}>Назад</button>
            </div>
        </div>
    );
};
