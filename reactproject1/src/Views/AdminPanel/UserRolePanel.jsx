import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";
import { AgGridReact } from "@ag-grid-community/react";
import PopUpWindow from "./PopUpWindow";
import { Typography,Grid,Box, FormControl, FormLabel, Button, InputLabel, Input } from "@mui/material";

import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';
import DeleteUserRole from "./DeleteRole";


class RoleRender extends React.Component
{
    constructor(props)
    {
        super(props);    
    }

    render()
    {
        return <Typography>{this.props.value}</Typography>
    }
}

export default class UserRolePanel extends React.Component
{

    deleteUserRole(id)
    {
        return <DeleteUserRole id={id}/>
    }

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
                    'PopUp':[this.deleteUserRole]
                }},
                { field: 'id',
                headerName:"ID",
                cellRenderer:RoleRender
                },
                {
                    field:'description',
                    headerName:'Описание',
                    cellRenderer:RoleRender
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

        this.deleteUserRole= this.deleteUserRole.bind(this);

        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })

    }

    async GetRoles()
    {
        this.state.Request.get("/api/User/GetAllRoles").then((e)=>{
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

    async SendRole()
    {
        this.state.Request.post("api/User/AddRole",{
            id:0,
            description:this.state.NewRole
        }).then((e)=>{
            console.log(e);
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
                <Typography  textAlign={"center"}>
                    Все роли
                </Typography>
                <Grid item>
                <div className="ag-theme-alpine" style={{ height: 400, width: "100%" }}>
                <AgGridReact
                    columnDefs={this.state.columnDefs}
                    defaultColDef={this.state.defaultColDef}
                    autoGroupColumnDef={this.state.autoGroupColumnDef}
                    animateRows={true}
                    rowData={this.state.readyData}
                    onGridReady={(e)=>{this.GetRoles()}}
                />
                </div>
                </Grid>
            </Grid>
            <Grid item>
                <Box  
                display={"flex"} 
                component="form"
                margin={'auto'}
                flexDirection="column"
                justifyContent="center"
                alignItems="center"
                width={"400px"}
                border={"1px solid #000"}
                >
                    <FormLabel>
                        Добавить роль
                    </FormLabel>
                    <FormControl>
                        <InputLabel>Название роли</InputLabel>
                        <Input
                        required
                        value={this.state.NewRole}
                        onChange={(e)=>this.setState({NewRole:e.target.value})}
                        id="input-password"/>
                    </FormControl>
                    <FormControl>
                        <Button onClick={(e)=>this.SendRole()}>Подтвердить</Button>
                    </FormControl>
                </Box>
            </Grid>
        </Grid>
        </div>
    }

}