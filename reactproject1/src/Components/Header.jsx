import { Toolbar,Box,Stack,CssBaseline,AppBar, IconButton, Drawer, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Divider } from "@mui/material";
import React from "react";
import { Link } from "react-router-dom";
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';


import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';


export default class Header extends React.Component
{

    toggleDrawer(event,value)
    {
        if (event.type === 'keydown' && (event.key === 'Tab' || event.key === 'Shift'))
            return;
        this.setState({menuIsOpen:value})
    }


    constructor(props)
    {
        super(props)
        this.state={menuIsOpen:false};
    }



    render()
    {

        return ( 
        <Box sx={{ display: 'flex' }}>
            <CssBaseline/>
            <AppBar position="fixed" >
                <Toolbar>
                        <IconButton 
                        edge='start'
                        onClick={e=>this.toggleDrawer(e,true)}
                        >
                            <MenuIcon/>
                        </IconButton>
                    <Stack margin='auto' direction='row' spacing={2} >
                        {(this.props.navigationMenu)?
                        this.props.navigationMenu.map((item,index)=>(
                            
                        <Link
                            to={item.url}
                            className="nav-bar-list"
                        >
                            {item.value}
                       </Link>
                    )):null}
                    </Stack>
                </Toolbar>
                </AppBar>
                <Drawer
                    anchor="left"
                    open={this.state.menuIsOpen}
                    onClose={(e)=>this.toggleDrawer(e,false)}>
                        <IconButton onClick={(e)=>this.toggleDrawer(e,false)}>
                            <ChevronLeftIcon/>
                        </IconButton>
                    <Divider/>
                    <List>
                        {this.props.dropMenu.map((item)=>(
                            <ListItem>
                                <Link to={item.url}  style={{textDecoration:'none'}}>
                                <ListItemButton>
                                    <ListItemText>
                                        {item.value}
                                    </ListItemText>
                                </ListItemButton>
                                </Link>
                            </ListItem>
                        ))}
                    </List> 
                </Drawer> 
                
        </Box>);
    }
}