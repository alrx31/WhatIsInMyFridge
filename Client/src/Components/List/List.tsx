import React, { useContext, useEffect } from "react";
import "./List.scss";
import { useNavigate } from "react-router-dom";
import { Context } from "../..";
import AuthService from "../../services/AuthService";
import FridgeService from "../../services/FridgeService";
import { IFridge } from "../../models/Fridge";

interface IListProps {}

export const List: React.FC<IListProps> = () => {
    let { store } = useContext(Context);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [fridges, setFridges] = React.useState<IFridge[]>([]);
    let history = useNavigate();

    const logoutHandle = async () => {
        try {
            await AuthService.logout(store.user.Id);
            history('/login');
        } catch (e) {
            console.error(e);
        }
    };

    const getFridges = async () => {
        try {
            const response = await FridgeService.getFridgesByUserId(store.user.Id);
            if (response.status === 200) {
                const data: IFridge[] = response.data.map((item: any) => ({
                    Id: item.id,
                    Name: item.name,
                    Model: item.model,
                    Serial: item.serial,
                    BoughtDate: item.boughtDate.split('T')[0],
                    BoxNumber: item.boxNumber
                }));
                setFridges(data);
            }
        } catch (e: any) {
            console.error(e);
            history('/');
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        getFridges();
    }, []);

    return (
        <div className="list-page">
            <h2>Your Fridges</h2>

            <div className="list-page__buttons">
                <button className="button__logout" onClick={logoutHandle}>
                    LogOut
                </button>
                <button onClick={() => history(`/user/${store.user.Id}`)}>
                    Profile
                </button>
            </div>

            <ul className="list-page__list">
                {fridges.length === 0 ? (
                    <h1>Fridges Not Found</h1>
                ) : (
                    fridges.map((fridge) => (
                        <div key={fridge.Id} className="fridge_item">
                            <h2 className="fridge_name">Name: {fridge.Name}</h2>
                            <p className="fridge_model">Model: {fridge.Model}</p>
                            <p className="fridge_serial">Serial: {fridge.Serial}</p>
                            <p className="fridge_boughtDate">Bought Date: {fridge.BoughtDate}</p>
                            <p className="fridge_boxNumber">Box Number: {fridge.BoxNumber}</p>
                        </div>
                    ))
                )}
            </ul>
        </div>
    );
};
