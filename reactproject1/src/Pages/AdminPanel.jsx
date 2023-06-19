import * as React from "react";
import axios from "axios";
import { Box } from "@mui/system";
import { AppBar, autocompleteClasses, Button, CssBaseline, Divider, Drawer, IconButton, List, ListItem, ListItemButton, ListItemIcon, ListItemText, ListSubheader, Switch, Toolbar, Typography } from "@mui/material";
import MenuIcon from '@mui/icons-material/Menu';
import { Link, Route, Router } from "react-router-dom";
import SettingsIcon from '@mui/icons-material/Settings';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';
import AddTeacher from '../Components/AdminPanel/AddTeacher'
import AddClass  from "../Components/AdminPanel/AddClass";
import { useEffect } from "react";
import PutchTeacher from "../Components/AdminPanel/PutchTeacher";

const baseURL = 'http://localhost:3001';
const URL='/Admin';



export class AdminPanel extends React.Component{

    constructor(props)
    {
        super(props)
        this.state={pagePanel:0,
            rightSideBar:false,
        }
    }

    SetPagePanel(index)
    {
        this.setState({pagePanel:index});
    }

    toggleRightBar()
    {
        this.setState({rightSideBar:!this.state.rightSideBar})
    }

    render()
    {
        return(
            <>
            <Box sx={{ display: 'flex' }}>
                    <Drawer
                        anchor="left"
                        open={true}
                        sx={{
                            width: 200,
                            flexShrink: 0,
                            [`& .MuiDrawer-paper`]: { width: 200, boxSizing: 'border-box' },}}
                            variant="persistent">
                        <Toolbar />
                        <Box sx={{ overflow: 'auto' }}>
                        <List>
                            {['Inbox', 'Starred', '2', '3', '4', '5'].map((text, index) => (
                            <ListItem key={text} disablePadding>
                            <ListItemButton key={text} onClick={(e)=>(this.SetPagePanel(index))}>
                                <ListItemIcon>
                                {index % 2 === 0 ? <InboxIcon /> : <MailIcon />}
                                </ListItemIcon>
                            <ListItemText primary={text} />
                            </ListItemButton>
                            </ListItem>))}
                        </List>
                        <Divider />
                             <List>
                                {['All mail', 'Trash', 'Spam'].map((text, index) => (
                                    <ListItem key={text} disablePadding>
                                        <ListItemButton>
                                            <ListItemIcon>
                                            {index % 2 === 0 ? <InboxIcon /> : <MailIcon />}
                                            </ListItemIcon>
                                            <ListItemText primary={text} />
                                        </ListItemButton>
                                    </ListItem>
                                ))}
                            </List>
                        </Box>
                    </Drawer>
                    

                    <Box component="main" sx={{overflow: 'auto', flexGrow: 1, bgcolor: 'background.default', p: 3 }}>
                    <Toolbar />
                        {(()=>{
                        switch(this.state.pagePanel)
                        {
                            case 0:
                                break;
                                
                            case 1:
                              
                                return <AddTeacher lang={this.props.lang? this.props.lang.allowedLanguages:null} request={this.props.request} />;
                                break;
                            case 2:
                        
                                return <PutchTeacher/>
                                break;
    
                            default:
                                return <AddClass />
                                break;
                                
                        }
                        })()}
                    </Box> 
            </Box>
            </>
        )
    }

} 
    