import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";
import { AgGridReact } from "@ag-grid-community/react";
import PopUpWindow from "./PopUpWindow";
import { Typography,Grid,Box, FormControl, FormLabel, Button, InputLabel, Input, Autocomplete, TextField } from "@mui/material";

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

class OutputsRender extends React.Component
{
    constructor(props)
    {
        super(props);

        console.log(props);
        this.state={
            Groups:this.props.value,
            AllowedOutputs:[],
            Request:null,     
            loading:false  
        }
        this.state.loading=this.state.AllowedOutputs===0;

        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })
    }

    async DeleteOutputGroup(Groups)
    {
        let diffrent = this.state.Groups.filter(i=>!Groups.includes(i))
        .concat(Groups.filter(i=>!this.state.Groups.includes(i)));
        console.log(diffrent);  
        let response =  await this.state.Request.delete("/api/Group/OutputGroupRole",{
            data:{
                roleId:this.props.data.id,
                groupId:diffrent[0].id,
            }
        });

        console.log(response);
        switch(response.data?.statusCode)
        {
            case 200:{
                
                this.setState({Groups:Groups});
                break;
            }
        }

    }


    async AddoutputToGroup(Groups)
    {
        console.log(Groups);
        console.log(this.props.data);
        let response =  await this.state.Request.post("/api/Group/OutputGroupRole",
        {
            roleId:this.props.data.id,
            groupId:Groups[Groups.length-1].id
        });

        console.log(response);
        switch(response.data?.statusCode)
        {
            case 200:
            {
                this.setState({Groups:Groups});
                break;
            }
        }
    }


    UpdateHeight()
    {

    }

    
    render()
    {
        return (
            this.state.AllowedOutputs?
            <Autocomplete
                multiple
                loading={this.state.loading}
                onChange={(e,value,reson)=>{
                    switch(reson)
                    {
                        case 'selectOption':{
                            this.AddoutputToGroup(value);
                            break;
                        }
                        case 'removeOption':{
                            this.DeleteOutputGroup(value);
                            break;
                        }
                        case 'clear':{

                            break;
                        }
                    }

                    this.UpdateHeight();
                }}
                onOpen={(e)=>{
                    let groups= this.props.context.GetAllowedOutputGroup();
                    this.setState({AllowedOutputs:groups});
                }}
                isOptionEqualToValue={(option,value)=>option?.id?.toString()===value?.id?.toString()}
                value={this.state.Groups}
                options={this.state.AllowedOutputs}
                getOptionLabel={(option) => option.description} 
                renderInput={(params) => (
                    <TextField
                      {...params}
                      key={params.id}
                      variant="standard"
                      label="Группы выходов"
                      placeholder="Выберите группу"
                    />
                  )}
            >
                
            </Autocomplete>:null
        )
    }
}


class ControllerGroupsRender extends React.Component
{
    constructor(props)
    {
        super(props);

        console.log(props);
        this.state={
            Groups:this.props.value,
            AllowedOutputs:[],
            Request:null,     
            loading:false  
        }
        this.state.loading=this.state.AllowedOutputs===0;

        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })
    }

    async DeleteOutputGroup(Groups)
    {
        let diffrent = this.state.Groups.filter(i=>!Groups.includes(i))
        .concat(Groups.filter(i=>!this.state.Groups.includes(i)));
        console.log(diffrent);  
        let response =  await this.state.Request.delete("/api/Group/ControllerGroupRole",{
            data:{
                roleId:this.props.data.id,
                groupId:diffrent[0].id,
            }
        });

        console.log(response);
        switch(response.data?.statusCode)
        {
            case 200:{
                
                this.setState({Groups:Groups});
                break;
            }
        }

    }


    async AddoutputToGroup(Groups)
    {
        console.log(Groups);
        console.log(this.props.data);
        let response =  await this.state.Request.post("/api/Group/ControllerGroupRole",
        {
            roleId:this.props.data.id,
            groupId:Groups[Groups.length-1].id
        });

        console.log(response);
        switch(response.data?.statusCode)
        {
            case 200:
            {
                this.setState({Groups:Groups});
                break;
            }
        }
    }

    
    render()
    {
        return (
            this.state.AllowedOutputs?
            <Autocomplete
                multiple
                loading={this.state.loading}
                onChange={(e,value,reson)=>{
                    switch(reson)
                    {
                        case 'selectOption':{
                            this.AddoutputToGroup(value);
                            break;
                        }
                        case 'removeOption':{
                            this.DeleteOutputGroup(value);
                            break;
                        }
                        case 'clear':{

                            break;
                        }
                    }
                }}
                onOpen={(e)=>{
                    let groups= this.props.context.GetAllowedControllerGroup();
                    this.setState({AllowedOutputs:groups});
                }}
                isOptionEqualToValue={(option,value)=>option?.id?.toString()===value?.id?.toString()}
                value={this.state.Groups}
                options={this.state.AllowedOutputs}
                getOptionLabel={(option) => option.description} 
                renderInput={(params) => (
                    <TextField
                      {...params}
                      key={params.id}
                      variant="standard"
                      label="Группы контроллеров"
                      placeholder="Выберите контроллеров"
                    />
                  )}
            >
                
            </Autocomplete>:null
        )
    }


}

export default class UserRolePanel extends React.Component
{

    deleteUserRole(id,context)
    {
        return <DeleteUserRole id={id} context={context}/>
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
                {
                    field:"ControllerGroup",
                    headerName:"Группы контроллеров",
                    autoHeight: true,
                    cellRenderer:ControllerGroupsRender
                },
                {
                    field:"OutputGroup",
                    headerName:"Группы выходов",
                    autoHeight: true,
                    cellRenderer:OutputsRender
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
            rerender:false,

        }

        this.deleteUserRole= this.deleteUserRole.bind(this);
        this.UpdateUserRole = this.GetRoles.bind(this);

        this.state.Request = axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })

        this.GetOutputGroup=this.GetOutputGroup.bind(this);
        this.GetControllerGroup=this.GetControllerGroup.bind(this);
        this.GetAllowedOutputGroups=this.GetAllowedOutputGroups.bind(this);
        this.GetAllowedControllerGroup=this.GetAllowedControllerGroup.bind(this);
    }


    GetOutputGroup()
    {
        return this.state.AllowedOutputGroup;
    }

    GetControllerGroup()
    {
        return this.state.AllowedControllerGroup;
    }

    async GetAllowedOutputGroups()
    {
        let response = await this.state.Request.get("/api/Group/OutputGroup");
        switch(response.data.statusCode)
        {
            case 200:{
                let m2mRoleOutput =[];
                let data=[];
                response.data.value.forEach((e)=>{
                    let tmp={}
                    tmp.id=e.id;
                    tmp.description=e.description;
                    data.push(tmp);
                    let RoleOutput={}

                    RoleOutput.Group=e.id;
                    RoleOutput.Roles=e.userRoles;
                    m2mRoleOutput.push(RoleOutput);
                })

                this.setState({m2mRoleOutput:m2mRoleOutput})
                this.setState({AllowedOutputGroup:data})

                break;
            }
        }
    }

    async GetAllowedControllerGroup()
    {
        let response = await this.state.Request.get("/api/Group/ControllerGroup");
        console.log(response);
        switch(response.data.statusCode)
        {
            case 200:{
                let data=[];
                let m2mRoleController =[];

                response.data.value.forEach((e)=>{
                    let tmp={}
                    tmp.id=e.id;
                    tmp.description=e.groupDescription;
                    data.push(tmp);

                    let RoleController={};
                    RoleController.id=e.id;
                    RoleController.Roles=e.userRoles;
                    m2mRoleController.push(RoleController);
                })
                this.setState({m2mRoleController:m2mRoleController});
                this.setState({AllowedControllerGroup:data})
                break;
            }
        }
    }

    async GetRoles()
    {


        this.state.Request.get("/api/User/GetAllRoles").then((e)=>{
            console.log(e);
            switch(e.data.statusCode)
            {
                case 200:{
                    let data =[];
                    e.data.value.forEach((element)=>{
                        let tmp = {};
                        tmp.id=element.id;
                        tmp.description=element.description;
                        tmp.OutputGroup=[];
                        tmp.ControllerGroup=[];
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

    onGridready(params)
    {
        this.state.gridApi = params.api;
        this.state.gridColumnApi = params.columnApi;
        this.GetRoles();
    }

    async SendRole()
    {
        this.state.Request.post("api/User/AddRole",{
            id:0,
            description:this.state.NewRole
        }).then((e)=>{
            this.GetRoles();
        })
    }

    componentDidMount()
    {
        this.GetAllowedControllerGroup();
        this.GetAllowedOutputGroups();
    }

    render()
    {
        if(this.state.m2mRoleController !== undefined && this.state.m2mRoleOutput!== undefined && this.state.readyData!=undefined && !this.state.rerender)
        {
            console.log('rerender');
            let data = this.state.readyData;
            for(let i=0;i<this.state.m2mRoleOutput.length;i++)
            {
                data.forEach(element => {
                    let role = this.state.m2mRoleOutput[i].Roles.find(x=>x.id==element.id)
                    if(role !== undefined)
                    {
                        if(element.OutputGroup.find(x=>x.id===this.state.AllowedOutputGroup[i].id)===undefined)
                            element.OutputGroup.push(this.state.AllowedOutputGroup[i]);
                    }
                });
               
                
            }
            this.state.gridApi.setRowData(data);
            console.log(data);

        }
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
                    context={{
                    "refresh":this.UpdateUserRole,
                    "GetAllowedOutputGroup":this.GetOutputGroup,
                    "GetAllowedControllerGroup":this.GetControllerGroup,
                    "GridApi":this.state.gridApi,             
                    }}
                    getRowId={(e)=>{return e.data.id}}
                    rowData={this.state.readyData}
                    onGridReady={(e)=>{this.onGridready(e)}}
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