import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";
import { Route, Routes } from "react-router";
import LogOut from "../../Pages/LogOut";
import UserValue from "./UserValue";


const DropDown =[{url:"/User/LogOut",value:"Выход"}]; 

export default class ControllerGrid extends React.Component
{
    constructor(props)
    {
        super(props)
        this.state={Request:null}
        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            responseType: 'stream'
        })
        console.log(props);
        this.props.ChangeDropDownMenu(DropDown); 
    }

    componentWillUnmount()
    {
        this.props.ChangeDropDownMenu(undefined);
    }

    render()
    {
        console.log('sa');
        return <div>
            <Routes>
                <Route path="/LogOut/*" element={<LogOut />} />
                <Route path="/*" element={<UserValue/>} />
            </Routes>
        </div>
    }
}