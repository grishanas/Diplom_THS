import { Box, Button, Dialog, DialogContent, MenuItem, Modal, Select, TextField, Typography } from "@mui/material";
import axios from "axios";
import React, { createRef } from "react";
import { BaseUrl } from "../../App";
import Tippy from "@tippyjs/react";

export default class QueryDialog extends React.Component
{
    constructor(props)
    {
        super(props);
        this._text="Изменить запрос";
        console.log(props);
        this.state={Request:null};
        this.state={visible:false,hide:true,AllowedQueris:undefined,newRequest:undefined}
        if(this.props.value!==undefined)
            this.state={hide:false};   
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        });

        this.GetQuery();
        this.GetAlllowedQuerys();
    }

    async GetAlllowedQuerys()
    {
        let response = await this.state.Request.get("/api/ControllerOutput/GetAllAllowQeurys?address="+this.props.id.controllerAddress);
        console.log(response.data);
        switch(response.data.statusCode)
        {
            case 200:{
                let queris=JSON.parse(response.data.value);
                console.log(queris);
                let tmp=[];
                tmp.push(queris);
                console.log(tmp);
                this.setState({AllowedQueris:tmp});
                break;
            }
        }
        console.log(response);
    }

    async GetQuery()
    {
        let response = await this.state.Request.get("/api/ControllerOutput/Output/"+this.props.id.id +"?adrress="+this.props.id.controllerAddress);
        console.log(response.data.value);
        switch(response.data.statusCode)
        {
            case 200:{
                let query= JSON.parse(response.data.value.query.query);
                this.setState({description:query.descriptions,startAddress:query.startAddress,shift:query.bitshift});
                break;
            }
        }
    }

    async setQeury()
    {
        let query=this.state.AllowedQueris.find(x=>x.descriptions === this.state.newRequest);
        query.startAddress= this.state.newStartAddress;
        query.bitshift= this.state.newShift;
         let response = await this.state.Request.post("/api/ControllerOutput/AddQeuryToOutput",{
            address:this.props.id.controllerAddress,
            outputId:this.props.id.id,
            qeury: JSON.stringify(query)
        });

        console.log(response);
    }


    Content()
    {
        return <Dialog 
        open={this.state.visible}
        onClose={(e)=>this.setState({visible:false})}>
            <DialogContent> 
            <Box display={"flex"} flexDirection={"column"} >
                <Box flexDirection={"column"}>
                <Box flexDirection={"column"}>
                    <Typography>Текущий запрос:</Typography>
                    <Box flexDirection={"row"}>
                        <Typography>{this.state.description}</Typography>
                        <Typography>{this.state.startAddress}</Typography>
                        <Typography>{this.state.shift}</Typography>
                    </Box>
                </Box>
                {console.log(this.state)}
                {this.state.AllowedQueris?
                <Box flexDirection={"column"} display={"flex"}>
                    <Select
                        value={this.state.newRequest}
                        placeholder="Команда"
                        onChange={(e)=>{this.setState({newRequest:e.target.value})}}
                    >
                        {this.state.AllowedQueris?this.state.AllowedQueris.map((e)=>(
                            <MenuItem value={e.descriptions}>
                                {e.descriptions}
                            </MenuItem>
                        )):null}
                    </Select>
                    <TextField
                        variant="outlined"
                        label="Начальный адрес слова"
                        value={this.state.newStartAddress}
                        onChange={(e)=>{this.setState({newStartAddress:e.target.value})}}
                    />
                    <TextField
                        variant="outlined"
                        label="Начальный адрес бита"
                        value={this.state.newShift}
                        onChange={(e)=>{this.setState({newShift:e.target.value})}}
                    />   
                </Box> :null}
                </Box>
                <Box flexDirection={"row"}>
                    <Button onClick={async (e)=>{await this.setQeury();this.setState({visible:false})}}>Подтвердить</Button>
                    <Button onClick={(e)=>{this.setState({visible:false})}}>Отменить</Button>
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

                allowHTML={true}
                arrow={false}
                appendTo={document.body}
                interactive={true}
                placement="right">
                <Button onClick={(e)=>{
                    this.state.visible ? 
                        this.setState({visible:false})
                        :this.setState({visible:true})
                    this.GetQuery();
                    }}
                    >
                    {this._text}
                </Button>    
            </Tippy>
    }
}