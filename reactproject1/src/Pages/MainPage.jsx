import { Select,MenuItem } from "@mui/material"
import React from "react"
import { Route, Routes } from "react-router"
import { Footer } from "../Components/Footer"
import Navigation from "../Components/Navigation"
import { Classes } from "../Components/Сlasses"
import { Home } from "./Home"

import axios from "axios";

const baseURL = 'http://localhost:3001';

class MainPage extends React.Component{


    constructor(props)
    {
        super(props);
        this.state={request:axios.create({
            baseURL: baseURL,
            headers:{ 'Content-Type': 'application/json' },
        })}

        this.state.pageInformation=null;
    }


    updatePage()
    {
    
    }

    componentDidMount(prevProps)
    {
        this.updatePage()
    }

    componentDidUpdate(prevProps)
    {
        if(this.props.lang.pageLanguage!==prevProps.lang.pageLanguage)
        {
            this.updatePage()
        }
    }



    render()
    {
        return(
            <>  
                <Navigation lang={this.props.lang}
                    onLangChange={this.props.onLangChange}
                />
                <Routes>
                    <Route path='/' element={<Home  pageLanguage={this.props.lang.pageLanguage}/>} />
                    {//<Route path='Classes' element={<Classes pageLanguage={this.props.lang.pageLanguage}/>}/>
                    }
               </Routes>
                <Footer/>  
            </>
        )
    }

}

export default MainPage