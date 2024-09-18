import React, { useContext } from "react";
import "./List.scss";
import { useNavigate } from "react-router-dom";
import { Context } from "../..";
interface IListProps {
}
export const List:React.FC<IListProps> = ({

})=>{

    let {store} = useContext(Context);
    let history = useNavigate();

    return (
        <div className={"list-page"}>

            <h2>Your Fridges</h2>
            
            <div className="list-page__buttons">
                <button className="button__logout"
                
                >LogOut</button>
                <button
                onClick={()=>history(`/user/${store.user.Id}`)}
                >Profile</button>
            </div>


            <ul className="list-page__list">
                {// will be list
                }
            </ul>
        </div>
    )
}