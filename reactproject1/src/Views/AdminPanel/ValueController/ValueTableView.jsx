import React from "react";
import axios from "axios";
import { AgGridReact } from "@ag-grid-community/react";

import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';
import { BaseUrl } from "../../../App";
import { Box, Button,Grid, Card, Dialog, DialogContent, FormControl, FormLabel, Typography, InputLabel, Input } from "@mui/material";


export default class ValueTableView extends React.Component
{
    constructor(props)
    {
        super(props)

        this.state={
            columnDefs:[
                { 
                    field: 'id',
                    headerName:"ID",
                },
                {
                    field:'name',
                    headerName:'Имя',
                },
                {
                    field:'description',
                    headerName:'Описание',
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

        this.state.request = axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })
    }


    async RequestData()
    {
        let response = await this.state.request.get("/api/Value/StartListen");
        let streamReader= response.body.getReader();

        while(true)
        {
            const {done, value} = await streamReader.read();

            if (done) {
              break;
            }
          
            console.log(value)
        }
    }

    render()
    {
        return <div>
            <Grid
                width="100%"
                container
                direction="column"
                justifyContent="center"
                alignItems="stretch"
            >
                <Grid item>
                    <Typography textAlign={"center"}>
                        Названия контрроллеров
                    </Typography>
                    <Grid item>
                        <div className="ag-theme-alpine" style={{ minHeight: 400,height:400, width: "100%" }}>
                            <AgGridReact
                                columnDefs={this.state.columnDefs}
                                defaultColDef={this.state.defaultColDef}
                                autoGroupColumnDef={this.state.autoGroupColumnDef}
                                animateRows={true}
                                rowData={this.state.readyData}
                                context={{'Request':this.state.Request}}
                                onGridReady={(e)=>{this.RequestData()}}
                            />
                        </div>
                    </Grid>
                </Grid>
            </Grid>
        </div>
    }

}