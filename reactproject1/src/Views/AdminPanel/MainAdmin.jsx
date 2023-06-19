import React from "react";
import { Route } from "react-router"
import { Routes} from "react-router-dom"
import Users from "./Users";
import AddController from "../AddController";
import UserRolePanel from "./UserRolePanel";
import ControllerPanelRender from "./Controller/ControllerPanel";
import ControllerName from "./Controller/ControllerName";
import ValueTableView from "./ValueController/ValueTableView";
import ControllerGroup from "./Controller/ControllerGroup";
import LogOut from "../../Pages/LogOut";
import ValuePanel from "../ValuePanel/Value";

//const Headers= [{url:"/Admin/Users",value:"Пользователи"},{url:"/AddRole",value:"Добавить группу пользователей"},{url:"/AddController",value:"Добавить контроллер"},{url:"/All"}];
const DropDown =[{url:"/Admin/Users",value:"Пользователи"},{url:"/Admin/Roles",value:"Роли пользователей"},{url:"/Admin/Controller",value:"Контроллеры"},
    {url:"/Admin/ControllerGroup",value:"Группировка контроллеров"},{url:"/Admin/LogOut",value:"Выход"}]; 

export default class AdminPage extends React.Component
{
    constructor(props)
    {
        super(props);
        props.ChangeDropDownMenu(DropDown); 
    }

    componentWillUnmount()
    {
        this.props.ChangeDropDownMenu(undefined);
    }

    render()
    {
        return(
            <>
            <Routes>
                <Route path="/Users/*" element={<Users />}/>
                <Route path="/Roles/*" element={<UserRolePanel />}/>
                <Route path="/Controller/*" element={<ControllerPanelRender />}/>
                <Route path="/ControllerGroup/*" element={<ControllerGroup />}/>
                <Route path="/LogOut/*" element={<LogOut />} />
                <Route path="/*" element={<ValuePanel/>}/> 
            </Routes>
            </>
        )
    }
}