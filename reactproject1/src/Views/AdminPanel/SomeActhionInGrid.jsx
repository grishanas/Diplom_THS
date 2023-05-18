import { Button, Dialog,DialogContent,Box,Typography } from "@mui/material";
import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";

export default class DeleteSomeThingInGrid extends React.Component{

    constructor(props)
    {
        super(props);
        this.state={visible:false,Request:null}

        this.state.Request= axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })
    }

    async delete()
    {
        this.state.Request.delete('/api/User/DeleteRole/'+this.props.id).then((e)=>{
            console.log(e);
        })
    }


    render()
    {
        return <div>
            <Button variant="text" onClick={(e)=>this.setState({visible:true})}>
                {this.props.text}
            </Button>
            <Dialog 
            open={this.state.visible}
            onClose={(e)=>this.setState({visible:false})}>
                <DialogContent> 
                <Box display={"flex"} flexDirection={"column"} >
                    <Typography> {this.props.text1 }</Typography>
                    <Box flexDirection={"row"}>
                        <Button onClick={(e)=>{this.delete();this.setState({visible:false}) }}>Подтвердить</Button>
                        <Button onClick={(e)=>{this.setState({visible:false})}}>Отменить</Button>
                    </Box>
                </Box>
                </DialogContent>
            </Dialog>
        </div>

    }
}