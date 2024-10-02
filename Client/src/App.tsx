import React, { useContext, useEffect, useState } from 'react';
import './App.scss';
import { Route, Routes, useNavigate } from 'react-router-dom';
import Login from './Components/Register/Login';
import Register from './Components/Register/Register';
import { Context } from './index';
import { observer } from 'mobx-react-lite';
import { Waiter } from './Components/Waiter/Waiter';
import { Profile } from './Components/Profile/Profile';
import { List } from './Components/List/List';
import { FridgePage } from './Components/Fridge/FridgePage';
import { AddFridgePage } from './Components/Fridge/AddFridgePage';
import { RecieptPage } from './Components/Reciept/RecieptPage';
import * as signalR from '@microsoft/signalr';

function App() {
  const { store } = useContext(Context);
  const navigate = useNavigate();
  const [expiringProducts, setExpiringProducts] = useState<{ message: string, id: number }[]>([]);

  useEffect(() => {
    store.checkAuth().finally(() => {
      if (!store.isAuth) {
        console.log("User is not authenticated. Redirecting to login.");
        navigate('/login');
      }
    });

    const token = localStorage.getItem('token');
    console.log("Retrieved token:", token);

    const connection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost/fridge/NotificationHub', {
        accessTokenFactory: () => token || ''
      })
      .withAutomaticReconnect()
      .build();

    connection.start()
      .then(() => {
        console.log('Connected to SignalR');

        connection.on('ReceiveNotification', (message) => {
          const id = Date.now(); // Уникальный идентификатор для уведомления
          setExpiringProducts(prev => [...prev, { message, id }]);
          console.log('Received notification:', message);

          // Удаляем уведомление через 5 секунд
          setTimeout(() => {
            setExpiringProducts(prev => prev.filter(product => product.id !== id));
          }, 5000);
        });

        console.log('Connection state:', connection.state);
      })
      .catch((error) => {
        console.error('SignalR connection failed:', error);
      });

    return () => {
      connection.stop().then(() => console.log('SignalR connection stopped.'));
    };
  }, [store, navigate]);

  if (store.isLoading) {
    return <Waiter />;
  }

  const logoutHandle = async () => {
    try {
      console.log('Logging out...');
      await store.logout();
    } catch (e) {
      console.error('Logout failed:', e);
    } finally {
      navigate('/login');
    }
  };

  return (
    <div className="App">
      <header>
        <h2 className="header__title" onClick={() => navigate('/')}>
          What Is In My Fridge
        </h2>

        <div className="list-page__buttons">
          <button className="button__logout" onClick={logoutHandle}>
            LogOut
          </button>
          <button onClick={() => navigate(`/user/${store.user.Id}`)}>
            Profile
          </button>
        </div>
      </header>

      {expiringProducts.length > 0 && (
        <div className="expiry-warning">
          <ul className="notification-wrapper">
            {expiringProducts.map((product, index) => (
              <li
               key={product.id}
              className="notification-item"
              >Will go bad: {product.message}</li>
            ))}
          </ul>
        </div>
      )}

      <Routes>
        <Route path="/" element={<List />} />
        <Route path="/fridges" element={<List />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/fridge/:FridgeId" element={<FridgePage />} />
        <Route path="/reciept/:RecieptId" element={<RecieptPage />} />
        <Route path="/add-fridge" element={<AddFridgePage />} />
        <Route path="/user/:UserId" element={<Profile />} />
      </Routes>
    </div>
  );
}

export default observer(App);
