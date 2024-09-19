import { useNavigate, useParams } from "react-router-dom";
import React, { useContext, useEffect, useState } from "react";
import { Context } from "../..";
import { IFridge } from "../../models/Fridge";
import FridgeService from "../../services/FridgeService";
import './FridgePage.scss';

interface IFridgePageProps {}

export const FridgePage: React.FC<IFridgePageProps> = () => {
    
    let {FridgeId} = useParams();

    const [isLoad, setIsLoad] = useState(true);

    let history = useNavigate();

    const {store} = useContext(Context);

    const [fridge, setFridge] = useState<IFridge>({} as IFridge);
    
    let getFridge = async () => {
        try {
            if(!store.user.Id) {
                history('/login');
                return;
            }
            const response = await FridgeService.getFridgeById(Number(FridgeId));
            if (response.status === 200) {
                const data: IFridge = {
                    Id: response.data.id,
                    Name: response.data.name,
                    Model: response.data.model,
                    Serial: response.data.serial,
                    BoughtDate: response.data.boughtDate.split('T')[0],
                    BoxNumber: response.data.boxNumber
                };
                setFridge(data);
            }
        } catch (e: any) {
            console.error(e);
            history('/');
        } finally {
            setIsLoad(false);
        }
    }

    useEffect(() => {
        getFridge();
    }, [FridgeId]);

    if (isLoad) {
        return <h2>Loading...</h2>;
    }



    
    return (
        <div 
            className="fridge-page"
        > 
            <h1>Fridge Page</h1>
            <h2>{fridge.Name}</h2>
            <h2>{fridge.Model}</h2>

        </div>
    );
}