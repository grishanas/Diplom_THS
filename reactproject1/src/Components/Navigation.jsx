import React,{useState} from "react";
import { AppBar, Toolbar,Box, CssBaseline, Typography, Stack, Select, MenuItem} from "@mui/material";
import { Link } from "react-router-dom";
import axios from "axios";

const baseURL = 'http://localhost:3001';


class Navigation extends React.Component
{
    constructor(props)
    {
        console.log(props)
        super(props)
        axios.defaults.withCredentials = true;
        this.state={request:axios.create({
            baseURL: baseURL,
            headers:{ 'Content-Type': 'application/json' },
        })}
        this.state.allowedLanguages=this.props.lang.allowedLanguages;
    }
    componentDidMount(prevProps)
    {
        this.state.request(`/NavigationMenu?key=${this.props.lang.pageLanguage}`).then((e)=>{
            this.setState({navigationMenu:e.data.navigationMenu});
        })
    }

    componentDidUpdate(prevProps)
    {
        if(this.props.lang!==prevProps.lang)
        {
            this.state.request(`/NavigationMenu?key=${this.props.lang.pageLanguage}`).then((e)=>{
                this.setState({navigationMenu:e.data.navigationMenu});
            })
            this.state.allowedLanguages=this.props.lang.allowedLanguages;
        }
    }

    changeLanguage(e)
    {
        this.props.onLangChange(e.target.value);
    }

    render()
    {
        return(
            <Box sx={{ display: 'flex' }}>
            <CssBaseline/>
            <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                <Toolbar>
                    <Box sx={{ display: { xs: 'none', sm: 'block' }}}>
                    <Link to="/" style={{ color:'#ff22f2',
                        textDecoration:'none',
                        }}>
                    <Typography >
                    GlobalITschool
                    </Typography>
                    </Link>  
                    </Box>
                    <Stack margin='auto' direction='row' spacing={2} >
                        {(this.state.navigationMenu)?
                        this.state.navigationMenu.map((item,index)=>(
                            
                            <Link
                            to={item.url}
                            className="nav-bar-list"
                            >
                                {item.value}
                            </Link>

                        )):null}

                    </Stack>
                    <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
                        <Select
                            onChange={(e)=>this.changeLanguage(e)}
                            defaultValue={this.props.lang.pageLanguage}
                        >
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
                </Toolbar>
            </AppBar>
            </Box>
            
        )
    }
}

export default Navigation




/*export const Navigation=(props)=>{

    return(
        <nav id="menu">
            <div className="container">
                <div className="navigationTabBar">
                    <div className="NavigationTitle">
                        <Link
                            to="/">
                            <h1>GlobalITschool</h1>
                        </Link>
                    </div>
                  { <ul className="nav-bar-list">
                       {props.Navigation.ref ? props.Navigation.ref.map((value)=>(
                        <li>
                        <Link to={value.link} className="NavBarComponent">
                            {value.text}
                        </Link>
                       </li>
                       
                       )):<li><p>Load</p></li>}
                       </ul>}

                    <div className="nav-bar-right-part">
                        {props.Navigation.SocialMedia ? props.Navigation.SocialMedia.map((value)=>(
                            <div className="nav-bar-SocialMedia">
                                <a href={value.link}>
                                    <img src={value.photo}/>
                                </a>
                            </div>
                        )):null}
                    </div>
                        
                </div>
            </div>

        </nav>
    )
}*/
