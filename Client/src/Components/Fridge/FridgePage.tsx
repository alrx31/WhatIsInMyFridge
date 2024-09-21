import { useNavigate, useParams } from "react-router-dom";
import React, { useContext, useEffect, useState } from "react";
import { Context } from "../..";
import { IFridge } from "../../models/Fridge";
import FridgeService from "../../services/FridgeService";
import './FridgePage.scss';
import { IProduct } from "../../models/Product";
import ProductService from "../../services/ProductService";

interface IFridgePageProps {}

export const FridgePage: React.FC<IFridgePageProps> = () => {
    
    let {FridgeId} = useParams();

    const [isLoad, setIsLoad] = useState(true);

    let history = useNavigate();

    const {store} = useContext(Context);

    const [fridge, setFridge] = useState<IFridge>({} as IFridge);
    const [products, setProducts] = useState<IProduct[]>([] as IProduct[]);
    


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

                const productsResponse = await ProductService.getProductsByFridgeId(Number(FridgeId));
                if (productsResponse.status === 200) {
                    console.log(productsResponse.data);
                    const productsData: IProduct[] = productsResponse.data.map((product: any) => {
                        return {
                            Id: product.id,
                            Name: product.name,
                            PricePerKilo: product.pricePerKilo,
                            ExpirationTime: product.expirationTime,
                            Count: product.count,
                            AddTime: product.addTime.split('T')[0]
                        };
                    });
                    setProducts(productsData);
                }
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
        <>
            <div 
                className="fridge-page"
            > 
                <h1>Fridge Page</h1>
                <h2>{fridge.Name}</h2>
                <h2>{fridge.Model}</h2>


                <ul>
                    {products.map((product) => {
                        return (
                            <li key={product.Id} className="list-product_item">
                                <h3>{product.Name}</h3>
                                <p>Count:  {product.Count}</p>
                                <p>When Add:  {product.AddTime}</p>
                                <p>Expiration Date:  {product.ExpirationTime}</p>
                                <p>PricePerKilo:  {product.PricePerKilo}</p>
                            </li>
                        );
                    })}
                </ul>


            </div>

            <div className="product-list-controlls">
                <button
                
                >Add Products</button>
                <button
                
                >Get Products</button>
                <button
                
                >Remove Products</button>
                <button
                
                >Go Back</button>
            </div>
        </>
    );
}