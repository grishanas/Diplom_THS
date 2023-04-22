import React from "react";
import { Route } from "react-router"
import { Routes} from "react-router-dom"
import Users from "./Users";
import AddController from "../AddController";
import UserRolePanel from "./UserRolePanel";
import ControllerPanel from "./ControllerPanel";

//const Headers= [{url:"/Admin/Users",value:"Пользователи"},{url:"/AddRole",value:"Добавить группу пользователей"},{url:"/AddController",value:"Добавить контроллер"},{url:"/All"}];
const DropDown =[{url:"/Admin/Users",value:"Пользователи"},{url:"/Admin/Roles",value:"Роли пользователей"},{url:"/Admin/Controller",value:"Контроллеры"},{url:"/All"}]; 

export default class AdminPage extends React.Component
{
    constructor(props)
    {
        super(props);
        props.ChangeDropDownMenu(DropDown); 
    }

    render()
    {
        return(
            <>
            <Routes>
                <Route path="/Users" element={<Users />}/>
                <Route path="/Roles" element={<UserRolePanel />}/>
                <Route path="/Controller" element={<ControllerPanel />}/>
            </Routes>
            </>
        )
    }
}