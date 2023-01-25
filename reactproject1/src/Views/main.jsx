import React from "react";
import { Route, Routes } from "react-router"
import Header from "../Components/Header";
import {Box,Toolbar} from "@mui/material"
import AddController from "./AddController";




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
           <Header/>
           <Toolbar/>
            <Routes>
                <Route path="/AddController" element={<AddController/>}/>
            </Routes>
        
        </>


        );
    }


}