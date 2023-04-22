import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button } from "@mui/material";
import React, { Component } from "react";
import PopUpWindow from "./PopUpWindow";
import axios from "axios";
import { BaseUrl } from "../../App";


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
                headerName:"ID",
                cellRenderer:Render
                },
                {
                    field:'description',
                    headerName:'Описание',
                    cellRenderer:Render
                },
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
        }
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        })

    }

    async GetController()
    {
        this.state.Request.get("/api/Controller/GetAll").then((e)=>{
            switch(e.status)
            {
                case 200:{
                    let data =[];
                    e.data.value.forEach((element)=>{
                        let tmp = {};
                        tmp.id=element.id;
                        tmp.description=element.description;
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
        console.log("dsa");
        return <duv>
            <Grid
            height="100%"
            width="100%"
            container
            direction="column"
            justifyContent="center"
            alignItems="stretch">
                <Grid item>
                    <Card>
                        <Grid container display={"flex"} flexDirection={"column"}>
                            <Grid item>
                                <Typography textAlign={"center"}>Контроллеры</Typography>
                            </Grid>
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
                    </Card>
                </Grid>
                <Grid item>
                    <Paper>
                        <Grid container>
                            <Grid item>
                                <Card>
                                <Box  
                                    display={"flex"} 
                                    component="form"
                                    margin={'0 auto'}
                                    flexDirection="column"
                                    justifyContent="center"
                                    alignItems="center"
                                    width={"400px"}
                                    border={"1px solid #000"}
                                >
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
                                            label="Выбирете описание контроллера"
                                            value={this.state.ControllerName}
                                        >
                                        </Select>
                                    </FormControl>
                                    <FormControl>
                                        <Button>
                                            
                                        </Button>
                                    </FormControl>
                                 </Box>
                                </Card>
                            </Grid>
                        </Grid>
                    </Paper>
                </Grid>

            </Grid>
        </duv>
    }
}