import React from "react";
import axios from "axios";
import { BaseUrl } from "../../../App";
import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button, MenuItem, Chip, OutlinedInput } from "@mui/material";
import PopUpWindow from "../PopUpWindow";


class OutputRole extends React.Component{
    constructor(props)
    {
        super(props)
        this.state ={Request:null,GroupName:null,DeleteGroup:null,Groups:undefined}
        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })
    }

    async GetGroups()
    {
        let responce = await this.state.Request.get("/api/Group/ControllerGroup");
        switch(responce.status)
        {
            case 200:
                {
                    
                    break;
                }
        }
    }

    async DeleteGroup(DeleteGroup)
    {

    }

    async AddGroup()
    {

    }

    render()
    {
        return <div>
            <Grid
                container
                justifyContent="center"
                direction="row"
            >
                <Grid item>
                    <Card>
                        <FormLabel>
                            Добавить группу выходов
                        </FormLabel>
                        <FormControl>
                            <InputLabel onChange={(e)=>this.setState({GroupName:e.target.value})}>Название группы</InputLabel>
                            <Input/>
                        </FormControl>
                        <FormControl>
                            <Button onClick={(e)=>{this.AddGroup()}}>
                                Добавит группу
                            </Button>
                        </FormControl>
                    </Card>

                </Grid>
                <Grid item>
                    <Card>
                        <FormLabel>
                            Удалить группу выходов
                        </FormLabel>
                        <FormControl>
                            <Select 
                                onFocus={()=>this.GetGroups()}
                                onChange={(e)=>this.setState({DeleteGroup:e.target.value})}
                                value={this.state.DeleteGroup}
                            >
                                {this.state.Groups?
                                this.state.Groups.map((e)=>(
                                    <MenuItem
                                        key={e.id}
                                        value={e}
                                    >
                                        {e.description}
                                    </MenuItem>
                                ))
                                :<Typography>Нет групп</Typography>}
                            </Select>

                        </FormControl>
                        <FormControl>
                            <Button onClick={(e)=>this.DeleteGroup()}>
                                Удалить
                            </Button>
                        </FormControl>
                    </Card>

                </Grid>

            </Grid>

        </div>
    }
}





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

export default class QueryPanel extends React.Component
{

    constructor(props)
    {
        console.log('Render Query');
        super(props);

        console.log(props);

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
                    headerName:'Описание выхода',
                    cellRenderer:Render
                },
                {
                    field:"name",
                    headerName:"Название выхода",
                    cellRenderer:Render
                },
                {
                    field:"ControllerGroups",
                    headerName:"Группы выходов",
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
        }
       
        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })
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

    async GetOutput()
    {
        console.log(this.state);
        let id = this.IPV4toInt(this.props.id);
        console.log(id);
        let responce = await this.state.Request.get("GetAllOutput/"+id);
        switch(responce.status)
        {
            case 200:
            {
                var data = []
                responce.data.value.forEach((e)=>{
                    let tmp = {};
                    tmp.id=e.id;
                    tmp.name=e.name;
                    tmp.description = e.description;
                    data.push(tmp);
                })
                this.setState({readyData:data});
                break;
            }
        }
    }

    render()
    {
        console.log("render");
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
                    Выходы контроллера: {this.props.id}
                </Typography>
                <Grid item>
                    <div className="ag-theme-alpine" style={{ height: 400, width: "100%" }}>
                        <AgGridReact
                            columnDefs={this.state.columnDefs}
                            defaultColDef={this.state.defaultColDef}
                            autoGroupColumnDef={this.state.autoGroupColumnDef}
                            animateRows={true}
                            rowData={this.state.readyData}
                            onGridReady={(e)=>{this.GetOutput()}}
                        />
                    </div>
                 </Grid>
            </Grid>
        </Grid>
        </div>
    }
}