import { Button, Menu, MenuItem } from "@mui/material";
import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";

export default class PopUpMenuRole extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state={anchor:null,Request:null}
        this.state.Request=axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        });
        console.log(props);
    }

    async deleteRole()
    {
        var userid=this.props.data.id?this.props.data.id:this.props.data.UserId;
        console.log(userid);
        this.state.Request.delete("/api/User/DeleteUserRole",{
            data:
            {
            userId:userid,
            roleId:this.props.data.userRoleId
            }
        }).then((e)=>{
            console.log(e);
        })
    }

    Close()
    {
        this.setState({anchor:null});
    }

    render()
    {
        return <div>
            <Button 
                sx={{color:"#000"}} 
                onClick={(e)=>this.setState({anchor:e.currentTarget})}
            >
                {this.props.value}
            </Button>
            <Menu
                open={Boolean(this.state.anchor)}
                anchorEl={this.state.anchor}
                onClose={(e)=>this.Close()}
            >
                <MenuItem onClick={(e)=>this.deleteRole()}>Удалить роль</MenuItem>
                
            </Menu>
        </div>
    }
}