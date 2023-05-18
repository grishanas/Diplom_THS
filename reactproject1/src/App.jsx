import { Grid, Paper, Typography } from '@mui/material';
import React, { Component } from 'react';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Header from './Components/Header';
import Aythorize from './Pages/Authorize';
import MainPage from './Views/main';
import AdminPage from './Views/AdminPanel/MainAdmin';
import ControllerGrid from './Views/UserPanel/controllerGrid';

export const BaseUrl="https://localhost:8977";
const Headers= [{url:"/AddController",value:"A"},{url:"B",value:"B"},{url:"C",value:"C"}];
const DropDownMenu = [{url:"/Aythorize",value:"Авторизация"}]

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = {dropDownMenu:DropDownMenu,navigationMenu:Headers,JWTToken:null,refreshJwtToke:null};
        this.ChangeNavMenu=this.ChangeNavMenu.bind(this);
        this.ChangeDropDownMenu= this.ChangeDropDownMenu.bind(this);
    }

    componentDidMount()
    {
        console.log("mount")
    }

    componentDidUpdate()
    {
        console.log("update")   
    }
    

    ChangeDropDownMenu(Menu)
    {
        if(Menu===undefined)
            Menu =DropDownMenu;
        console.log(Menu)
        this.setState({dropDownMenu:Menu})
    }

    ChangeNavMenu(newMenu)
    {
        

        this.setState({navigationMenu:newMenu});
    }

    render() {
        return (
        <Router>
            <Grid
                display={'flex'}   
                container
                width={'100%'}
                height={'100%'}
                direction="column"
                justifyContent="flex-start"
                alignItems="center">
                <Grid item height={"70px"}>
                    <Header dropMenu={this.state.dropDownMenu} navigationMenu={this.state.navigationMenu}/>
                </Grid>
                
                <Grid item
                width={"100%"}>
                    <Routes>    
                        <Route path="/*" element={<MainPage /> }/>
                        <Route path='/Aythorize' element={<Aythorize />}/>
                        <Route path="/User/*" element={<ControllerGrid />}/>
                        <Route path="/Admin/*" element={<AdminPage ChangeNavMenu={this.ChangeNavMenu} ChangeDropDownMenu={this.ChangeDropDownMenu}/>}/>
                    </Routes>
                </Grid>
            </Grid>
        </Router>
        );
    }

    // async populateWeatherData() {
    //     const response = await fetch('weatherforecast');
    //     const data = await response.json();
    //     this.setState({ forecasts: data, loading: false });
    // }
}
