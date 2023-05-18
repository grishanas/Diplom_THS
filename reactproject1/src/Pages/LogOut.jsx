import axios from "axios"
import React from "react"
import { BaseUrl } from "../App"
import { Navigate } from "react-router"
import react from 'react'


export default class LogOut extends react.Component
{
    constructor(props)
    {
        super(props)
        this.state={Request:null,redirect:false}
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })

    }

    redirect()
    {
        console.log('redirect');
        this.setState({redirect:false});
        return <Navigate to="/"/>
    }

    async LogOut()
    {
        var responce = await this.state.Request.post("/api/Authorization/LogOut");
        console.log(responce);
        switch(responce.status)
        {
            case 200:{
                    this.setState({redirect:true})
                break;
            }
        }
    }

    render()
    {
        this.LogOut();
        return <div> 
            {this.state.redirect?this.redirect():null}
        </div>

    }

}