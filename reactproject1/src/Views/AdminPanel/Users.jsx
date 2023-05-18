'use strict';


import { Box, Button, Card, FormControl, Grid, Input, InputLabel, MenuItem, OutlinedInput, Paper, Select, TextField, Typography } from "@mui/material";
import axios from "axios";
import React, { createRef } from "react";
import { BaseUrl } from "../../App";

import { AgGridReact } from '@ag-grid-community/react';
import { ModuleRegistry } from '@ag-grid-community/core';
import { ClientSideRowModelModule } from '@ag-grid-community/client-side-row-model';
import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';
import "./table.css"
import PopUpWindow from "./PopUpWindow";
import AddUserRole from "./AddUserRole";
import Chip from "@mui/material/Chip";
import DeleteUser from "./DeleteUser";
import PopUpMenuRole from "./PopUpMenuRole";

ModuleRegistry.registerModules([ ClientSideRowModelModule, ]);


class GroupCellRender{

    init(params) 
    {
    
        this.ui = document.createElement('div');
        this.ui.innerHTML =
          '<div class="show-name">' +
          params.value +
          '</div>';
    }
    
    getGui() {
        return this.ui;
    }
    
    refresh() {
        return false;
    }
}

export default class Users extends React.Component
{
    addUserRole(id,context){
        if(id===undefined)
        {
            return null;
        }
        
        return <AddUserRole id={id} GetRoles={context.GetRoles} GetUser={context.GetUser} RefreshGrid={context.RefreshGrid}/>
    };

    deleteUser(id,context)
    {
        return <DeleteUser id={id} RefreshGrid={context.RefreshGrid}/>
    }

    GetUser(id)
    {
        return this.state.rawUserData.find((elem)=>{
            if(elem.id==id)
                return elem;
        });
    }

    RefreshGrid()
    {
        this.GetUsersAsync();
    }

    GetRoles()
    {
        return this.state.rawRolesData;
    }

    constructor(props)
    {
        super(props); 

        this.state = {
            readyData: null,
            rawUserData:null,
            rawRolesData:null,
            addUserRoles:[],
            NewUserLogin:undefined,
            NewUserPassword:undefined,
            
            columnDefs: [
              {
                field:'RoleLength',
                headerName:"Действие",
                pinned: 'left',  
                colid:"action",    
                editable: false,
                maxWidth: 150,
                cellRenderer:PopUpWindow,
                cellRendererParams:{
                    'PopUp':[this.addUserRole,this.deleteUser],
                },
                rowSpan:(params)=>{return params.data.RoleLength},
                cellClassRules: {
                    'cell-span':"value !== undefined",
                    'group-cell': 'value !== undefined',
                },
              },
              { field: 'id',
                headerName:"ID",
                cellRenderer:GroupCellRender,
                rowSpan:(params)=>{return params.data.RoleLength},
                cellClassRules: {
                    'cell-span':"value !== undefined",
                    'group-cell': 'value !== undefined',
                },},
            
              { field: 'login', headerName:"Логин",
                cellRenderer:GroupCellRender,
                rowSpan:(params)=>{return params.data.RoleLength},
                cellClassRules: {
                  'cell-span':"value !== undefined",
                  'group-cell': 'value !== undefined',
              },
            
            },
              { field: 'password' , headerName:"Пароль",                
                cellRenderer:GroupCellRender,
                rowSpan:(params)=>{return params.data.RoleLength},
                cellClassRules: {
                    'cell-span':"value !== undefined",
                    'group-cell': 'value !== undefined',
              },},
              { field: 'userRoles', 
              headerName:"Роль",
              cellRenderer:PopUpMenuRole
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


        };

        this.state.Request=axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })

        this.addUserRole=this.addUserRole.bind(this);
        this.GetRoles=this.GetRoles.bind(this);
        this.GetUser=this.GetUser.bind(this);
        this.RefreshGrid=this.RefreshGrid.bind(this);
        this.SendNewUser = this.SendNewUser.bind(this); 
        this.deleteUser = this.deleteUser.bind(this);
    }


    async SendNewUser()
    {
        this.state.Request.post("/api/User/AddUser",{
            id:0,
            login:this.state.NewUserLogin,
            password:this.state.NewUserPassword,
            userRoles:this.state.addUserRoles
        }).then((e)=>{
            this.setState({NewUserLogin:undefined,NewUserPassword:undefined,addUserRoles:[]});          
            this.RefreshGrid();
            
        });
    }

    async GetRolesAsync()
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
                    
                    this.setState({rawRolesData:data})
                    
                    break;
                }default:{

                    break;
                }
            }
        })

        
    }


    async GetUsersAsync()
    {
        this.state.Request.get("/api/User/GetAllUsers").then((e)=>
        {
            switch(e.status)
            {
                case 200:{
                    let data = [];
                    let rawdata =[];

                    e.data.value.forEach((element,i)=>{
                        let tmp = {};
                       
                        tmp.id = element.id;
                        tmp.login=element.login;
                        tmp.password = element.password;
                        rawdata.push({'id':element.id,'login':element.login,'password':element.password});
                        rawdata[i].userRoles=[];
                        for(let j=0 ; j<element.userRoles.length;j++)
                        {
                            rawdata[i].userRoles.push({'id':element.userRoles[j].id,'description':element.userRoles[j].description})
                        }

                        tmp.RoleLength=1;
                        if(element.userRoles.length>0)
                            for(let i = 0; i<element.userRoles.length;i++)
                            {
                                let temp = {};
                                Object.assign(temp,tmp); 
                                temp.userRoleId= element.userRoles[i].id;
                                temp.userRoles= element.userRoles[i].description;
                                data.push(temp);
                            }
                        else
                        {
                            data.push(tmp);
                        }
                        
                    });
                    this.setState({rawUserData:rawdata});
                    this.setState({readyData:data});
                    break;
                }
                default:{
                    break;
                }
            }
        })
    }

    ChangeSelectedRole(e)
    {
        this.setState({addUserRoles:e.target.value})

    }

    render()
    {
        return <Paper>
            <Grid
                width="100%"
                container
                direction="column"
                justifyContent="center"
                alignItems="stretch">
                    <Grid item>              
                        <Typography  textAlign={"center"}>
                          Все пользователи
                        </Typography>
                        <Grid item>
                            <div className="ag-theme-alpine" style={{ minHeight: 400,height:400, width: "100%" }}>
                                <AgGridReact
                                    columnDefs={this.state.columnDefs}
                                    defaultColDef={this.state.defaultColDef}
                                    autoGroupColumnDef={this.state.autoGroupColumnDef}
                                    context={{
                                        'GetUser':this.GetUser,
                                        'GetRoles':this.GetRoles,
                                        'RefreshGrid':this.RefreshGrid}}
                                    onGridReady={e=>{this.GetUsersAsync();this.GetRolesAsync()}}
                                    animateRows={true}
                                    rowData={this.state.readyData}
                                />
                            </div>
                        </Grid>
                    </Grid>        
                    <Grid item width={"100%"} >
                        <Card >
                            <Box  
                                display={"flex"} 
                                component="form"
                                flexDirection="column"
                                justifyContent="center"
                                alignItems="center"
                            >
                                <FormControl >
                                    <InputLabel>Имя пользователя</InputLabel>
                                        <Input
                                            required
                                            value={this.state.NewUserLogin}
                                            onChange={(e)=>this.setState({NewUserLogin:e.target.value})}
                                            id="input-name"
                                        />
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Пароль</InputLabel>
                                        <Input
                                            required
                                            value={this.state.NewUserPassword}
                                            onChange={(e)=>this.setState({NewUserPassword:e.target.value})}
                                            id="input-password"
                                        />
                                    </FormControl >
                                    <FormControl sx={{width:"180px"}}>
                                        <InputLabel>роль</InputLabel>
                                        <Select
                                            multiple
                                            placeholder="роль"
                                            id="add-select-role"
                                            value={this.state.addUserRoles}
                                            onChange={(e)=>{
                                                this.setState({addUserRoles:e.target.value})
                                            }}
                                            input={<OutlinedInput  id="select-multiple-role" label="Роли" />}
                                            renderValue={(selected)=>(
                                                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5,width:"100px" }}>
                                                {selected.map((element) =>(
                                                    <Chip label={element.description} key={element.id}/>  
                                                )    
                                                )}
                                                </Box>
                                            )} 
                                        >
                                            {this.state.rawRolesData?
                                                this.state.rawRolesData.map((element)=>(
                                                    <MenuItem
                                                        key={element.id}
                                                        value={element}
                                                    >
                                                        {element.description}
                                                    </MenuItem> 
                                                ))
                                            :<MenuItem>load</MenuItem>}
                                        </Select>
                                        </FormControl>
                                        <FormControl>
                                            <Button onClick={this.SendNewUser}>Подтвердить</Button>
                                        </FormControl>
                                    
                                    </Box>
                        </Card>

                    </Grid>

                </Grid>
            
            </Paper>
        
    }

}