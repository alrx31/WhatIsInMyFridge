import React, { useContext, useEffect } from "react";
import "./List.scss";
import { useNavigate } from "react-router-dom";
import { Context } from "../..";
import AuthService from "../../services/AuthService";
import FridgeService from "../../services/FridgeService";
import { IFridge } from "../../models/Fridge";
import { IUser } from "../../models/User";

interface IListProps {}

export const List: React.FC<IListProps> = () => {
    let { store } = useContext(Context);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [fridges, setFridges] = React.useState<IFridge[]>([]);
    let history = useNavigate();

    

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
        } finally {
            setIsLoading(false);
        }
    };

    let AddFridgeHandle = ()=>{
        history('/add-fridge');
    }

    useEffect(() => {
        getFridges();
    }, []);

    return (
        <div className="list-page">
            

            <ul className="list-page__list">
                {fridges.length === 0 ? (
                    <h1>Fridges Not Found</h1>
                ) : (
                    fridges.map((fridge,indx) => (
                        <div 
                            key={indx} 
                            className="fridge_item"
                            onClick={()=>{history(`/fridge/${fridge.Id}`)}}
                        >
                            <h2 className="fridge_name">Name: {fridge.Name}</h2>
                            <p className="fridge_model">Model: {fridge.Model}</p>
                            <p className="fridge_serial">Serial: {fridge.Serial}</p>
                            <p className="fridge_boughtDate">Bought Date: {fridge.BoughtDate}</p>
                            <p className="fridge_boxNumber">Box Number: {fridge.BoxNumber}</p>
                        </div>
                    ))
                )}
            </ul>

            <footer>
            <button
                className="get-reciept-button"
            >Get Reciept Suggest</button>
            <button
                className="get-list-button"
            >Lets Shop</button>
            <button
                className="add-fridge-button"
                onClick={AddFridgeHandle}
            >AddFridge</button>
            </footer>
        </div>
    );
};