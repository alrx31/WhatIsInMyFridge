import React, { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Context } from "../..";
import { IReciept } from "../../models/Reciept";
import RecieptService from "../../services/RecieptService";
import { Waiter } from "../Waiter/Waiter";
import './Reciept.scss';
import { IProduct } from "../../models/Product";

interface IRecieptProps {}

export const RecieptPage: React.FC<IRecieptProps> = () => {
    
    let {RecieptId} = useParams();

    const [isLoad, setIsLoad] = useState(true);

    let history = useNavigate();

    const {store} = useContext(Context);

    const [showDeletePopup, setShowDeletePopup] = useState(false);

    const [reciept, setReciept] = useState<IReciept>({} as IReciept);


    let getReciept = async () => {
        try {
                       
            if (!RecieptId) {
                console.error("RecieptId is undefined");
                
                return;
            }
            
            const response = await RecieptService.getRecieptById(RecieptId);
            
            if (response.status === 200) {
                const data: IReciept = {
                    id: response.data.id,
                    name: response.data.name,
                    cookDuration: response.data.cookDuration,
                    portions: response.data.portions,
                    kkal: response.data.kkal,
                    products: response.data.products.map((product: any) => {
                        return {
                            Id: product.id,
                            Name: product.name,
                            Count: product.weight,
                            PricePerKilo: product.pricePerKilo,
                            ExpirationTime: product.expirationTime,
                            AddTime: "1"
                        }}) as IProduct[]
                };
            
                setReciept(data);
            }
        } catch (e) {
            console.error(e);
        }
    };


    useEffect(() => {
        getReciept().then(() => {
            setIsLoad(false);
        });


    }, [RecieptId]);

    if (isLoad) {
        return <Waiter />;
    }

    if(!reciept.id) {
        return <div>Reciept not found</div>;
    }

    return (
       <>
        <div className="reciept-container">
            <h1 className="reciept-title">{reciept.name}</h1>
            <div className="reciept-info">
                <p>Cook Duration: {reciept.cookDuration}</p>
                <p>Portions: {reciept.portions}</p>
                <p>Calories: {reciept.kkal} kcal</p>
            </div>

            {store.user.IsAdmin && (
                <div className="reciept-page-controlls">
                <button
                
                >DEL</button>
                
                
                    <button
                    
                    >EDIT</button>
                
                </div>
            )}
            <h2>Products</h2>

            <table className="product-table">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Weight (g)</th>
                        <th>Price per Kilo</th>
                        <th>Expiration Time</th>
                    </tr>
                </thead>
                <tbody>
                    {reciept.products.map((product) => (
                        <tr key={product.Id}>
                            <td>{product.Name}</td>
                            <td>{product.Count} g</td>
                            <td>{product.PricePerKilo} $/kg</td>
                            <td>{product.ExpirationTime}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            
        </div>
        
        <div className="reciept-page-footer-controls">
            <button
                onClick={() => history('/')}
            >Back</button>
        </div>
        </>

    );
}

