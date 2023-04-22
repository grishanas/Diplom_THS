import { Box, Button, Dialog, DialogContent, Modal, Typography } from "@mui/material";
import axios from "axios";
import React, { createRef } from "react";
import { BaseUrl } from "../../App";
import Tippy from "@tippyjs/react";


export default class DeleteUser extends React.Component
{
    constructor(props)
    {
        super(props);
        this._text="Удалить пользователя";
        this.state={Request:null};
        this.state={visible:false,hide:true,AvalibelRoles:null}
        if(this.props.value!==undefined)
            this.state={hide:false};   
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        });
    }
    async deleteUser()
    {
        this.state.Request.delete('/api/User/DeleteUser/?id='+this.props.id).then((e)=>{
            console.log(e);
        })
    }

    Content()
    {
        return <Dialog 
        open={this.state.visible}
        onClose={(e)=>this.setState({visible:false})}>
            <DialogContent> 
            <Box display={"flex"} flexDirection={"column"} >
                <Typography> Вы точно хотите удалить пользователя ?</Typography>
                <Box flexDirection={"row"}>
                    <Button onClick={(e)=>{console.log(this);this.deleteUser();this.setState({visible:false}) }}>Подтвердить</Button>
                    <Button onClick={(e)=>{console.log('dsa');this.setState({visible:false})}}>Отменить</Button>
                </Box>
            </Box>
            </DialogContent>
        </Dialog>
    }

    render()
    {
        return <Tippy
                content={this.Content()}
                visible = {this.state.visible}
                onClickOutside={(e)=>this.state.visible ? this.setState({visible:false}): null}
                allowHTML={true}
                arrow={false}
                appendTo={document.body}
                interactive={true}
                placement="right">
                <button onClick={(e)=>{this.state.visible ? this.setState({visible:false}):this.setState({visible:true})}}>
                    {this._text}
                </button>    
            </Tippy>

    }
}