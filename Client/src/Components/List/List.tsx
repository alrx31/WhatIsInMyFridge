import React, { useContext, useEffect } from "react";
import "./List.scss";
import { useNavigate } from "react-router-dom";
import { Context } from "../..";
import FridgeService from "../../services/FridgeService";
import { IFridge } from "../../models/Fridge";
import { IProduct } from "../../models/Product";
import RecieptsService from "../../services/RecieptService";
import ProductsService from "../../services/ProductService";

interface IListProps {}

export const List: React.FC<IListProps> = () => {
    
    let { store } = useContext(Context);
    const [isLoading, setIsLoading] = React.useState<boolean>(true);
    const [fridges, setFridges] = React.useState<IFridge[]>([]);
    let history = useNavigate();

    const [product, setProduct] = React.useState({} as IProduct);
    const [addProduct, setAddProduct] = React.useState(false);

    
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

    let getRecieptSuggest = async () =>{
        var products = [] as IProduct[];

        for(let f in fridges){
            const response = await FridgeService.getProductsByFridgeId(fridges[f].Id);
            console.log(response);
            if (response.status === 200) {
                const data: IProduct[] = response.data.map((item: any) => ({
                    Id: item.id,
                    Name: item.name,
                    PricePerKilo: item.pricePerKilo,
                    ExpirationTime: item.expirationTime,
                    Count: item.count,
                    AddTime: item.addTime.split('T')[0]
                }));
                products = products.concat(data);
            }
        }

        try{
            const response = await RecieptsService.getRecieptSuggest(products);
        
            if (response.status === 200) {
                history(`/reciept/${response.data.id}`);
            }else{
                alert("Reciept not found");
            }
        }catch(e:any){
            console.error(e);
            alert("Reciept not found");
        }
    }

    useEffect(() => {
        getFridges();
    }, []);


    let addProductHandle = async () => {
        if(product.Name === undefined || product.PricePerKilo === undefined || product.ExpirationTime === undefined){
            alert("Fill all fields");
            return;
        }
        try {
            const response = await ProductsService.addProduct(product);
            if (response.status === 200) {
                alert("Product added");
            }
        } catch (e: any) {
            console.error(e);
            alert("Product not added")
        }finally{
            setProduct({} as IProduct);
            setAddProduct(false);
        }
    }

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
                onClick={getRecieptSuggest}
            >Get Reciept Suggest</button>
            <button
                className="get-list-button"
            >Lets Shop</button>
            <button
                className="add-fridge-button"
                onClick={AddFridgeHandle}
            >AddFridge</button>
            {!store.user.IsAdmin && (
                <button
                className="admin-panel-button"
                onClick={() => setAddProduct(true)}
                >
                    Add Product
                </button>
            )}
            </footer>

            {addProduct && (
               <>
               <div className="add-product-back-panel"></div>
                <div className="add-product">
                    <input
                        type="text"
                        placeholder="Name"
                        value={product.Name}
                        onChange={(e) => setProduct({ ...product, Name: e.target.value })}
                    />
                    <input
                        type="number"
                        placeholder="PricePerKilo"
                        value={product.PricePerKilo}
                        onChange={(e) => setProduct({ ...product, PricePerKilo: +e.target.value })}
                    />
                    <input
                        type="text"
                        placeholder="ExpirationTime"
                        value={product.ExpirationTime}
                        onChange={(e) => setProduct({ ...product, ExpirationTime: e.target.value })}
                    />
                    <button onClick={addProductHandle}>Add</button>
                    <button onClick={()=>setAddProduct(false)}>Cansel</button>
                </div>
                </>
            )}
        </div>
    );
};