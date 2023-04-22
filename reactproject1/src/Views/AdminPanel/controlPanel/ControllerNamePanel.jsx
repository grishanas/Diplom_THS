import React from "react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button } from "@mui/material";
import PopUpWindow from "../PopUpWindow";

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

export default class ControlerNamePanel extends React.Component
{
    constructor(props)
    {
        super(props)
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
            </Grid>
        </div>
    }
}