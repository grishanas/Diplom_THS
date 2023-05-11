import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button, MenuItem } from "@mui/material";
import React, { Component } from "react";
import PopUpWindow from "../PopUpWindow";
import axios from "axios";
import { BaseUrl } from "../../../App";


class Render extends React.Component{

    constructor(props)
    {
        super(props);
    }

    render()
    {
        return <Typography>{this.props.value}</Typography>
    }
}

export default class ControllerPanel extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state = {Request:null};
        this.state = {
            columnDefs:[
                {    
                field:'action',            
                headerName:"Действие",
                pinned: 'left',  
                colId:"action",    
                editable: false,
                maxWidth: 150,
                cellRenderer:PopUpWindow,
                cellRendererParams:{
                    'PopUp':[]
                }},
                { field: 'id',
                headerName:"IPV4",
                cellRenderer:Render
                },
                {
                    field:'description',
                    headerName:'Описание',
                    cellRenderer:Render
                },
                {
                    field:"name",
                    headerName:"Название контроллера",
                    cellRenderer:Render
                }
            ],
            defaultColDef: {
                flex: 1,
                minWidth: 100,
                sortable: true,
                resizable: true,
            },
            autoGroupColumnDef: {
                minWidth: 200,
            },
            readyData:undefined,
            controllerName:undefined,

            ipV4:undefined,
            name:undefined,
            description:undefined,
            ControllerName:undefined
        }
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })



    }

    async SendController()
    {

    }

    IPV4toInt(ip)
    {
        let parts=ip.split(".");
        let res=0;

        if(parts.length!=3)
            return null;
        res += parseInt(parts[0], 10) << 24;
        res += parseInt(parts[1], 10) << 16;
        res += parseInt(parts[2], 10) << 8;
        res += parseInt(parts[3], 10);

        return res;
    }

    IntToIPV4(int)
    {
        let part1=int&255;
        let part2=(int>>8)&255;
        let part3=(int>>16)&255;
        let part4=(int>>24)&255;
        return part4.toString()+"."+part3.toString()+"."+part2.toString()+"."+part1.toString();
    }
    async GetControllerNames()
    {
        let responce = await this.state.Request.get("api/ControllerName/AllowedControllerName");
        switch(responce.status)
        {
            case 200:
                {
                    let data=[];
                    responce.data.value.forEach((element)=>{
                        let tmp ={};
                        tmp.name= element.name;
                        tmp.values=element.values
                        data.push(tmp);
                    })
                    this.setState({controllerName:data});
                    break;
                }
        }
    }
    async GetController()
    {
        this.GetControllerNames();
        this.state.Request.get("/api/Controller/GetAll").then((e)=>{
            switch(e.status)
            {
                case 200:{
                    let data =[];
                    e.data.value.forEach((element)=>{
                        let tmp = {};
                        console.log(element);
                        tmp.id=this.IntToIPV4(element.ipAddress);
                        tmp.description=element.description;
                        tmp.name = element.controllerName.name+"  "+element.controllerName.version;
                        data.push(tmp);
                    })
                    
                    this.setState({readyData:data})
                    
                    break;
                }default:{

                    break;
                }
            }
        })
    }

    render()
    {
        return <div>
            <Grid
            height="100%"
            width="100%"
            container
            direction="column"
            justifyContent="center"
            alignItems="stretch">
                <Grid item>
                    <Typography textAlign={"center"}>
                        Контроллеры
                    </Typography>
                    <Grid item>
                        <div className="ag-theme-alpine" style={{ height: 400, width: "100%" }}>
                            <AgGridReact
                                columnDefs={this.state.columnDefs}
                                defaultColDef={this.state.defaultColDef}
                                autoGroupColumnDef={this.state.autoGroupColumnDef}
                                animateRows={true}
                                rowData={this.state.readyData}
                                onGridReady={(e)=>{this.GetController()}}
                            />
                        </div>
                     </Grid>
                </Grid>
                <Grid item width={"100%"} >
                    <Card>
                                <Box  
                                    style={{margin:"0 auto"}}
                                    display={"flex"} 
                                    component="form"
                                    flexDirection="column"
                                    justifyContent="center"
                                    alignItems="center"
                                    width={"400px"}
                                    border={"1px solid #000"}>
                                    <FormLabel>
                                        Добавить контроллер
                                    </FormLabel>
                                    <FormControl>
                                        <InputLabel>Адрес</InputLabel>
                                        <Input/>
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Имя</InputLabel>
                                        <Input/>
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Описание</InputLabel>
                                        <Input/>
                                    </FormControl>
                                    <FormControl sx={{ m: 1, minWidth: 200 }}>
                                        <InputLabel>Описание контроллера</InputLabel>
                                        <Select 
                                            label="Выбирете тип контроллера"
                                            value={this.state.ControllerName}
                                        >
                                            {this.state.controllerName ? this.state.controllerName.map((element)=>(
                                                element.values.map((e)=>(
                                                    <MenuItem
                                                        key={element.name + " " + e}
                                                        value={element.name + " " + e}
                                                    >
                                                        {element.name + " " + e}
                                                    </MenuItem>
                                                ))
                                            )) : null}
                                        </Select>
                                    </FormControl>
                                    <FormControl>
                                        <Button onClick={(e)=>{this.SendController()}}>
                                            Добавить контроллер
                                        </Button>
                                    </FormControl>
                        </Box>
                    </Card>
                </Grid>

            </Grid>
        </div>
    }
}