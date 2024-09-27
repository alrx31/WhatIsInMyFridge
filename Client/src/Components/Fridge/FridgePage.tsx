import { useNavigate, useParams } from "react-router-dom";
import React, { useContext, useEffect, useState } from "react";
import { Context } from "../..";
import { IFridge } from "../../models/Fridge";
import FridgeService from "../../services/FridgeService";
import './FridgePage.scss';
import { IProduct } from "../../models/Product";
import ProductsService from "../../services/ProductService";
import { set } from "mobx";

interface IFridgePageProps { }

export const FridgePage: React.FC<IFridgePageProps> = () => {

    let { FridgeId } = useParams();

    const [isLoad, setIsLoad] = useState(true);

    let history = useNavigate();

    const { store } = useContext(Context);

    const [showDeletePopup, setShowDeletePopup] = useState(false);
    const [deleteProdut, setDeleteProduct] = useState(false);
    const [deleteProductValue, setDeleteProductValue] = useState({} as IProduct);
    const [deleteProductCount, setDeleteProductCount] = useState(0);

    const [fridge, setFridge] = useState<IFridge>({} as IFridge);
    const [products, setProducts] = useState<IProduct[]>([] as IProduct[]);


    const [addProduct, setAddProduct] = useState(false);
    const [AllProducts, setAllProducts] = useState([] as IProduct[]);
    const [page, setPage] = useState(1);
    const [chousedOneProduct, setChousedOneProduct] = useState({} as IProduct);
    const [chouseProductCountPage, setChouseProductCountPage] = useState(false);
    const [chouseProductCount, setChouseProductCount] = useState(0);

    const [choosedProducts, setChoosedProducts] = useState([] as IProduct[]);

    let getFridge = async () => {
        try {

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

                const productsResponse = await FridgeService.getProductsByFridgeId(Number(FridgeId));
                if (productsResponse.status === 200) {
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


    let deleteFridgeHandle = async () => {
        try {
            let response = await FridgeService.deleteFridgeFromUser(store.user.Id, Number(FridgeId));
            if (response.status === 200) {
                history('/');
            } else {
                alert('Error deleting the fridge');
            }

        } catch (e: any) {
            console.error(e);
            alert('Error deleting the fridge');
        }
    }


    let handleGetProduct = async () => {
        if (deleteProductCount <= 0) {
            alert('Count must be greater than 0');
            return;
        }

        if (deleteProductCount > deleteProductValue.Count) {
            alert('Count must be equals or be less than product count');
            return;
        }

        try {
            let response = await FridgeService.removeProductFromFridgeWithCount(Number(FridgeId), deleteProductCount, deleteProductValue.Id);
            if (response.status === 200) {
                setDeleteProduct(false);
                getFridge();
            } else {
                alert('Error getting the product');
            }

        } catch (e: any) {
            console.error(e);
            alert('Error getting the product');
        }
    }

    let getAllProducts = async () => {
        try {
            let response = await ProductsService.getProducts(page, 10);
            if (response.status === 200) {
                const data: IProduct[] = response.data.map((product: any) => {
                    return {
                        Id: product.id,
                        Name: product.name,
                        PricePerKilo: product.pricePerKilo,
                        ExpirationTime: product.expirationTime,
                        Count: 0,
                        AddTime: ""
                    };
                });
                setAllProducts(data);
            } else {
                alert('Error getting the products');
            }
        } catch (e: any) {
            console.error(e);
            alert('Error getting the products');
        }
    }

    let handleChoiseProduct = async () => {
        setChouseProductCountPage(false);
        if (chouseProductCount <= 0) {
            alert('Count must be greater than 0');
            return;
        }

        let choosedProduct = choosedProducts.find((product) => product.Id === chousedOneProduct.Id);

        if (choosedProduct) {
            choosedProduct.Count += chouseProductCount;
        } else {
            choosedProduct = {
                Id: chousedOneProduct.Id,
                Name: chousedOneProduct.Name,
                PricePerKilo: chousedOneProduct.PricePerKilo,
                ExpirationTime: chousedOneProduct.ExpirationTime,
                Count: chouseProductCount,
                AddTime: chousedOneProduct.AddTime
            } as IProduct;

            setChoosedProducts([...choosedProducts, choosedProduct]);
        }
    }

    let handleAddProducts = async () => {
        if (choosedProducts.length === 0) {
            alert('You must choose at least one product');
            return;
        }

        try {
            let response = await FridgeService.addProductsToFridge(Number(FridgeId), choosedProducts);
            if (response.status === 200) {
                setAddProduct(false);
                getFridge();
            } else {
                alert('Error adding the products');
            }
        } catch (e: any) {
            console.error(e);
            alert('Error adding the products');
        }finally{
            setChoosedProducts([]);

        }
    }

    return (
        <>
            <div
                className="fridge-page"
            >
                <h1>{fridge.Name}</h1>
                <h2>Model: {fridge.Model}</h2>
                <h2>Serial: {fridge.Serial}</h2>
                <br />
                <button
                    onClick={() => setShowDeletePopup(true)}
                >Remove frige</button>


                <ul>
                    {products.map((product) => {
                        return (
                            <li key={product.Id} className="list-product_item">
                                <h3>{product.Name}</h3>
                                <p>Count:  {product.Count}</p>
                                <p>When Add:  {product.AddTime}</p>
                                <p>Expiration Date:  {product.ExpirationTime}</p>
                                <p>PricePerKilo:  {product.PricePerKilo}</p>
                                <button
                                    className="delete-product"
                                    onClick={() => {
                                        setDeleteProductValue(product);
                                        setDeleteProduct(true);
                                    }}
                                >GET</button>
                            </li>
                        );
                    })}
                </ul>


            </div>

            <div className="product-list-controlls">
                <button
                    onClick={() => {
                        setAddProduct(true);
                        getAllProducts();
                    }}
                >Add Products</button>
                <button

                >Remove Products</button>
                <button
                    onClick={() => history('/')}
                >Go Back</button>
            </div>

            {showDeletePopup && (
                <div className="popup-overlay">
                    <div className="popup">
                        <h2>Confirm Deletion</h2>
                        <p>Are you sure you want to delete this fridge?</p>
                        <div className="popup-buttons">
                            <button onClick={deleteFridgeHandle} className="confirm-button">Yes</button>
                            <button onClick={() => setShowDeletePopup(false)} className="cancel-button">No</button>
                        </div>
                    </div>
                </div>
            )}

            {deleteProdut && (
                <div className="popup-overlay">
                    <div className="popup">
                        <h2>Get Product {deleteProductValue.Name}</h2>

                        <p>Enter Count</p>
                        <input
                            type="number"
                            value={deleteProductCount}
                            onChange={(e) => setDeleteProductCount(Number(e.target.value))}
                        />
                        <div className="popup-buttons">
                            <button onClick={handleGetProduct} className="confirm-button">Get</button>
                            <button onClick={() => setDeleteProduct(false)} className="cancel-button">Cansel</button>
                        </div>
                    </div>
                </div>
            )}

            {addProduct && (
                <>
                <div className="add-product-back-panel"></div>
                <div className="add-products-page">
                    <div className="products-panel">
                        {AllProducts && AllProducts.map((product) => (
                            <div
                                key={product.Id}
                                className="product-item"
                                onClick={() => {
                                    setChousedOneProduct(product);
                                    setChouseProductCountPage(true);
                                }}
                            >
                                <h3>{product.Name}</h3>
                                <p>Exp. Time:  {product.ExpirationTime}</p>
                            </div>
                        ))}
                    </div>
                    <div className="products-panel">
                        {choosedProducts && choosedProducts.map((product) => (
                            <div key={product.Id} className="product-item">
                                <h3>{product.Name}</h3>
                                <p>Count:  {product.Count}</p>
                            </div>
                        ))}
                    </div>

                    <div className="add-products-page-controlls">
                        <button
                            onClick={handleAddProducts}
                        >Add Products</button>
                        <button
                            onClick={() => setAddProduct(false)}
                        >Cansel</button>
                    </div>
                    {chouseProductCountPage && (
                        <div className="popup-overlay">
                            <div className="popup">
                                <p>Enter Count</p>
                                <input
                                    type="number"
                                    value={chouseProductCount}
                                    onChange={(e) => setChouseProductCount(Number(e.target.value))}
                                />
                                <div className="popup-buttons">
                                    <button onClick={handleChoiseProduct} className="confirm-button">Get</button>
                                    <button onClick={() => setChouseProductCountPage(false)} className="cancel-button">Cansel</button>
                                </div>
                            </div>
                        </div>
                    )}
                </div>
                </>
            )}
        </>
    );
}