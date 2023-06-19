import React from "react";
import axios from "axios"
import Header from "../Components/Header";
import { Classes } from "../Components/Сlasses";
import { Educators } from "../Components/Educators";
import { baseURL } from "../App";


export class Home extends React.Component{

    constructor(props)
    {
        super(props)
        this.state={request:axios.create({
            baseURL: baseURL,
            headers:{ 'Content-Type': 'application/json' },
        })}
    
    }

    updatePage()
    {   
        this.state.request.get(`/HomePage?key=${this.props.pageLanguage}`).then((e)=>{
            console.log(e.data.homePage);
            this.setState({pageInformation:e.data.homePage})
        })
    }

    componentDidMount(prevProps)
    {
        this.updatePage()
    }

    componentDidUpdate(prevProps)
    {
        if(this.props.pageLanguage!==prevProps.pageLanguage)
        {
            this.updatePage()
        }
    } 




    render()
    {
        return(
            <>
            <Header header={this.state.pageInformation?this.state.pageInformation.header:null} />
            <Classes classes={this.state.pageInformation?this.state.pageInformation.classes:null}/>
            
            </>
        )

    }

}