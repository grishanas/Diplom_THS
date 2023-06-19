import { Button, FormControl, OutlinedInput, InputLabel, Paper, TextField, IconButton, InputAdornment } from "@mui/material";
import React from 'react';
import { AppBar, autocompleteClasses, CssBaseline, Divider, Drawer,Box, List,Select,MenuItem, ListItem, ListItemButton, ListItemIcon, ListItemText, ListSubheader, Switch, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import './Style/Admin.css'  
import Visibility from '@mui/icons-material/Visibility'
import VisibilityOff from '@mui/icons-material/VisibilityOff';
import axios from 'axios'
import SettingsIcon from '@mui/icons-material/Settings';
import { Navigate, Route, Routes, useNavigate } from "react-router";
import { AdminPanel } from "./AdminPanel";
import { Link } from "react-router-dom";

const baseURL = 'http://localhost:3001';
const URL='/Admin/SigIn';



class Admin extends React.Component
{
    constructor(props)
    {
        super(props)
        console.log(props);
        this.state={
            password:'',
            nickName:'',
            showPassword:false,
            rightSideBarIsOpen:false,
            isAuthorization:false,
        }

        axios.defaults.withCredentials = true;
        this.state.request = axios.create({
            baseURL: baseURL,
            headers:{ 'Content-Type': 'application/json' },
        });
    }

    async SendLogIn(e)
    {
    e.preventDefault();
    this.state.request.post(URL+"/Authinfication",{login:this.state.nickName ? this.state.nickName : null,password:this.state.password ?this.state.password  : null}).
        then((e)=>{
            switch(e.status)
            {
            case 200:
                {
                    this.setState({isAuthorization:true})
                    localStorage.setItem("RefreshToken",e.data.RefreshToken);
                    
                    break;
                }
            case 401:
                {

                    break;
                }
            }
        });
    }

    componentDidMount()
    {
        if(localStorage.getItem('RefreshToken'))
        {
            this.state.request.post('/Admin/SigIn/Refresh',{RefreshToken:localStorage.getItem('RefreshToken')}).then((data)=>{
                switch(data.status)
                {
                    case 200:
                    {
                        this.setState({isAuthorization:true})
                        localStorage.setItem("RefreshToken",data.data.RefreshToken);

                        break;
                    }
                    case 400:
                    case 401:
                    {
                        this.setState({isAuthorization:false});
                        localStorage.removeItem('RefreshToken');
                    }
                    default:{

                    }
                }
            })

        }
        else
        {

        }
    }

    

    handleMouseDownPassword(event){
        event.preventDefault();
    };

    toggleRightBar()
    {
        this.setState({rightSideBarIsOpen:!this.state.rightSideBarIsOpen})
    }


    render()
    {
        return( 
        <div className="box">
            <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                <Toolbar>
                    <Box sx={{ display: { xs: 'none', sm: 'block' }}}>
                    <Link to="/" style={{ color:'#ff22f2',
                        textDecoration:'none',
                        }}>
                    <Typography >
                        sometext
                    </Typography>
                    </Link>  
                    </Box>
                    <Box sx={{ flexGrow: 1 }} />
                    <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
                        <IconButton onClick={(e)=>this.toggleRightBar(e)}>
                            <SettingsIcon />
                        </IconButton>
                    </Box>
                </Toolbar>
                    <Drawer open={this.state.rightSideBarIsOpen} anchor="right" onClose={(e)=>this.toggleRightBar(e)} 
                        sx={{zIndex:(theme)=>theme.zIndex.drawer+1}}>
                        <Box component="div" style={{
                            width: 250,
                            height: "100%",
                            display:'flex',
                            
                            justifyContent:'center'
                        }}>
                            <Box>
                            <List sx={{width:'100%'}}>
                            <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
                            <Select
                            onChange={(e)=>this.props.onLangChange(e.target.value)}
                            defaultValue={this.props.lang.pageLanguage}>

                            {(this.props.lang.allowedLanguages)?
                            this.props.lang.allowedLanguages.map((item,ibdex)=>(
                                <MenuItem 
                                key={item.key}
                                value={item.key}>
                                    {item.value}
                                </MenuItem>
                            )):<Typography>Load</Typography>
                            }   

                            </Select>
                        
                            </Box>

                            <ListItemButton onClick={this.CloseUserAccount}>
                                <ListItemText primary={this.props.RightSideBar ? this.props.RightSideBar.Exit:"Exit"}/>
                            </ListItemButton>
            

                            </List>

                        </Box>
                        </Box>
    
                        </Drawer>
            </AppBar>
            <Box component="main" sx={{overflow: 'auto', flexGrow: 1, bgcolor: 'background.default', p: 3 }}>
            <Toolbar />
        {this.state.isAuthorization?
        <AdminPanel lang={this.props.lang} request={this.state.request}/>
        :
        <Paper className="AdminMainPaper">
        <div className="AdminContainer">
        <FormControl className="AdminFormControl">
        <InputLabel htmlFor="NickNameid">NickName</InputLabel>
        <OutlinedInput

            label="NickName"
            id="NickNameid"
            onChange={(e)=>this.setState({nickName: e.target.value})}
        >
        </OutlinedInput>
        </FormControl>
        <FormControl className="AdminFormControl">
        <InputLabel htmlFor="Passwordid">Password</InputLabel>
        <OutlinedInput
            label="Password"
            value={this.state.password}
            type={this.state.showPassword ? 'text' : 'password'}
            onChange={(e)=>this.setState({'password':e.target.value})}
            id="Passwordid"
            endAdornment={ 
            <InputAdornment position="end">
                <IconButton
                    aria-label="toggle password visibility"
                    onClick={(e)=>this.setState({showPassword:!this.state.showPassword})}
                    //onMouseDown={handleMouseDownPassword}
                    edge="end"
                >
                {this.state.showPassword ? <VisibilityOff /> : <Visibility />}
                </IconButton>
            </InputAdornment>
            
        }
        >

        </OutlinedInput>
        </FormControl>
        <Button onClick={e=>(this.SendLogIn(e))}>
            Sign In
        </Button>
    
        </div>
    
        </Paper>}

    </Box>

    </div>
        );
    }

}

export default Admin