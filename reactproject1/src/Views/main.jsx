import React from "react";
import { Route, Routes } from "react-router"
import AddController from "./AddController";
import ValuePanel from "./ValuePanel/Value";
import Aythorize from "../Pages/Authorize";




export default class MainPage extends React.Component
{

    constructor(props)
    {
        super(props);
        
    }


    render()
    {
        return(
         <>
         <Aythorize/>
        
        </>


        );
    }


}