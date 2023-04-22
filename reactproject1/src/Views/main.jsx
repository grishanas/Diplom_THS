import React from "react";
import { Route, Routes } from "react-router"
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
            <Routes>
                <Route path="/AddController" element={<AddController/>}/>
            </Routes>
        
        </>


        );
    }


}