import React from "react";
import { Route, Routes } from "react-router"
import AddController from "./AddController";
import ValuePanel from "./ValuePanel/Value";




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
            <ValuePanel/>
            <Routes>
                <Route path="/AddController" element={<AddController/>}/>
            </Routes>
        
        </>


        );
    }


}