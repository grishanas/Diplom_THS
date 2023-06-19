import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, TextField,Autocomplete,Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button, MenuItem, Chip, OutlinedInput } from "@mui/material";
import React, { Component } from "react";
import PopUpWindow from "../PopUpWindow";
import axios from "axios";
import { BaseUrl } from "../../../App";
import QueryPanel from "./QueryPanel";
import { Navigate, Route, useNavigate } from "react-router"
import { Routes} from "react-router-dom"
import Tippy from "@tippyjs/react";
import { People } from "@mui/icons-material";


class Render extends React.Component{

    constructor(props)
    {
        super(props);
        console.log(props);
        
    }

    render()
    {
        return <Typography>{this.props.value}</Typography>
    }
}

class SelectRender extends React.Component
{
    IPV4toInt(ip)
    {
        
        let parts=ip.split(".");
        let res= new Uint32Array(4);

        if(parts.length!=4)
            return null;
        res += parseInt(parts[0], 10) << 24;
        res += parseInt(parts[1], 10) << 16;
        res += parseInt(parts[2], 10) << 8;
        res += parseInt(parts[3], 10);

        return res;
    }

    constructor(props)
    {
        super(props)
        
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
        console.log(this.props);
    }

    async DeleteControllerFromGroup(Groups)
    {
        let diffrent = this.state.Groups.filter(i=>!Groups.includes(i))
        .concat(Groups.filter(i=>!this.state.Groups.includes(i)));
        console.log(diffrent);  
        let response =  await this.state.Request.delete("/api/Controller/ControllerGroup",{
            data:{
            id:this.props.id,
            group:diffrent[0].id
            }
        });

        switch(response.data.statusCode)
        {
            case 200:{
                
                this.setState({Groups:Groups});
                break;
            }
        }

    }


    async AddControllerToGroup(Groups)
    {
        console.log(Groups[Groups.length-1].id);
        console.log(this.IPV4toInt(this.props.data.id.id));
        let response =  await this.state.Request.post("/api/Controller/AddControllerToGroup",
        {
            id:this.IPV4toInt(this.props.data.id.id),
            group:Groups[Groups.length-1].id
        });
        console.log(response);
        switch(response.data.statusCode)
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
                ref={ (divElement) => { this.divElement = divElement }}
                multiple
                loading={this.state.loading}
                onChange={(e,value,reson)=>{
                    switch(reson)
                    {
                        case 'selectOption':{
                            this.AddControllerToGroup(value);
                            break;
                        }
                        case 'removeOption':{
                            this.DeleteControllerFromGroup(value);
                            break;
                        }
                        case 'clear':{

                            break;
                        }
                    }

                }}
                onOpen={(e)=>{
                    let groups= this.props.context.GetAllowedGroup();
                    console.log(groups);
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

class QeuryRoute extends React.Component
{
    constructor(props)
    {
        super(props)
        this._text="Просмотр Запросов";
    }

    redirect()
    {
        this.props.context.ChangeQeuryPanel(this.props.id);
    }

    render()
    {
        return <div>
            <Button onClick={(e)=>{this.redirect()}}
            >{this._text}
            </Button>
        </div>

    }
}

export default class ControllerPanelRender extends React.Component
{
    constructor(props)
    {
        super(props);
        this.state={QueryID:undefined,QueryContext:undefined,redirect:false}

        this.ChangeQeuryPanel= this.ChangeQeuryPanel.bind(this);
    }

    ChangeQeuryPanel(id,context)
    {
        console.log('sadsa')
        this.setState({QueryID:id,QueryContext:context,redirect:true});
    }
    redirect()
    {
        this.setState({redirect:false});
        return <Navigate to="QueryPanel"/>
    }

    render()
    {
        return <div>
            <Routes>
                <Route path="/" element={<ControllerPanel ChangeQeuryPanel={this.ChangeQeuryPanel}/>}/>
                <Route path="/QueryPanel" element={<QueryPanel id={this.state.QueryID} ChangeQeuryPanel={this.ChangeQeuryPanel}/>} />
                
            </Routes>
            {this.state.redirect?this.redirect():null}
        </div>
    }
}


class ControllerPanel extends React.Component
{

    AddControllerGroup(id,context)
    {
        //return <AddRoleToGroup/>
    }

    GetControllerQuery(id,context)
    {
        console.log(context);
        return <QeuryRoute id={id} context={context}/>
    }

    DeleteController(id,context)
    {
        return <Button>Удалить контроллер</Button>
    }

    constructor(props)
    {

        super(props);
        this.AddControllerGroup= this.AddControllerGroup.bind(this);
        this.GetControllerQuery = this.GetControllerQuery.bind(this);
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
                    'PopUp':[this.GetControllerQuery,this.DeleteController]
                }},
                { field: 'ID',
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
                },
                {
                    field:"state",
                    headerName:"Состояние",
                    cellRenderer:Render,
                },
                {
                    field:"ControllerGroups",
                    headerName:"Группы контроллеров",
                    autoHeight: true,
                    cellRenderer:SelectRender
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
            ControllerName:'',
            ControllerVersion:'',
            ControllerGroups:[],
        }
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })

        this.GetControllerGroup=this.GetControllerGroup.bind(this);

    }

    async SendController()
    {
       
        let response = await this.state.Request.post("/api/Controller/AddController",{
            "ipAddress": this.IPV4toInt(this.state.ip),
            "description": this.state.ControllerOriginaldescription,
            "ipPort": parseInt(this.state.ipPort),
            "controllerState": this.state.ControllerState.description,
            "name": this.state.ControllerOriginalName,
            "controllerName": {
              "name": this.state.ControllerName.name,
              "version": this.state.ControllerVersion
            }
          }
        )

        switch(response.data.statusCode)
        {
            case 200:{
                this.GetController();
            }
        }
        
        
    }

    IPV4toInt(ip)
    {
        
        let parts=ip.split(".");
        let res=0;

        if(parts.length!=4)
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
       
        this.state.Request.get("/api/Controller/GetAll").then((e)=>{
            switch(e.status)
            {
                case 200:{
                    let data =[];
                    e.data.value.forEach((element)=>{
                        let tmp = {};
                        console.log(element);
                        tmp.ID=this.IntToIPV4(element.ipAddress);
                        tmp.description=element.description;
                        tmp.id={
                            id:tmp.ID,
                            name: element.controllerName.name,
                            version:element.controllerName.version
                        }
                        tmp.name = element.controllerName.name+"  "+element.controllerName.version;
                        tmp.state= element.controllerState.description;
                        tmp.ControllerGroups= element.controllerGroups;
                        data.push(tmp);
                    })
                    
                    this.setState({readyData:data})
                    
                    break;
                }default:{

                    break;
                }
            }
        })
        this.GetControllerNames();
        this.GetControllerGroup();
    }


    componentDidMount()
    {
        this.GetControllerGroups();
    }

    GetControllerGroup()
    {
        return this.state.AllowedGroup;
    }

    async GetControllerGroups()
    {
        let responce = await this.state.Request.get("/api/ControllerGroup/GetControllerGroup");
        console.log(responce);
        switch(responce.status)
        {
            case 200:{
                console.log(responce.data.value)
                let data =[];
                responce.data.value.forEach((e)=>{
                    let tmp={};
                    tmp.id= e.id;
                    tmp.description = e.groupDescription;
                    data.push(tmp);
                })

                this.setState({AllowedGroup:data});

                break;
            }
        }
    }

    async GetControllerState(version)
    {
        let responce = await this.state.Request.
            get("/api/ControllerState/GetAllowedControllerState?"+"Name="+this.state.ControllerName.name+"&Version="+version);
        switch(responce.status)
        {
            case 200:
            {
                let data=[];
                responce.data.value.forEach((e)=>{
                    let tmp = {};
                    console.log(e);
                    tmp.description = e.description;
                    data.push(tmp);
                })
                this.setState({controllerStates:data});
                break;
            }
        }
        this.GetControllerGroup();
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
                                context={{"GetAllowedGroup":this.GetControllerGroup,'ChangeQeuryPanel':this.props.ChangeQeuryPanel}}
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
                                        <Input onChange={(e)=>{this.setState({ip:e.target.value})}}/>
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Порт контроллера</InputLabel>
                                        <Input onChange={(e)=>{this.setState({ipPort:e.target.value})}}/>
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Имя</InputLabel>
                                        <Input onChange={(e)=>{this.setState({ControllerOriginalName:e.target.value})}}/>
                                    </FormControl>
                                    <FormControl>
                                        <InputLabel>Описание</InputLabel>
                                        <Input onChange={(e)=>{this.setState({ControllerOriginaldescription:e.target.value})}}/>
                                    </FormControl>
                                    <FormControl sx={{ m: 1, minWidth: 200 }}>
                                        <InputLabel>Производитель контроллера</InputLabel>
                                        <Select 
                                            value={this.state.ControllerName}
                                            onChange={(e)=>{
                                                console.log(e.target.value)
                                                this.setState({ControllerName:e.target.value})
                                            }}
                                            
                                        >
                                            {this.state.controllerName ? this.state.controllerName.map((element)=>(
                                                <MenuItem
                                                    key={element.name}
                                                    value={element}
                                                >
                                                    {element.name}
                                                </MenuItem>
                                            )) : null}
                                        </Select>
                                    <FormControl>
                                        <InputLabel>Версия контроллера</InputLabel>
                                        <Select
                                            value={this.state.ControllerVersion}
                                            onChange={(e)=>{
                                                this.setState({ControllerVersion:e.target.value})
                                                this.GetControllerState(e.target.value);
                                            }}
                                        >
                                            {this.state.ControllerName?
                                            this.state.ControllerName.values.map((e)=>(
                                                <MenuItem
                                                    key={e}
                                                    value={e}
                                                >
                                                    {e}
                                                </MenuItem>
                                            ))
                                            :
                                            <MenuItem>Введите производителя</MenuItem>}
                                        </Select>
                                    </FormControl>
                                    </FormControl>
                                    <FormControl sx={{ m: 1, minWidth: 200 }}>
                                        <InputLabel>Состояние контроллера</InputLabel>
                                        <Select
                                            value={this.state.ControllerState}
                                            onChange={(e)=>{
                                                this.setState({ControllerState:e.target.value});
                                            }}
                                        >
                                        {this.state.controllerStates?
                                        this.state.controllerStates.map((e)=>(
                                            <MenuItem
                                                key={e.id}
                                                value={e}
                                            >
                                                {e.description}
                                            </MenuItem>
                                        ))
                                        :
                                        <MenuItem>Ожидание ввода версии контроллера</MenuItem>}

                                        </Select>
                                    </FormControl>
                                    <FormControl sx={{ m: 1, minWidth: 200 }}>
                                        <InputLabel>Группы</InputLabel>
                                        <Select
                                        multiple
                                        value={this.state.ControllerGroups}
                                        onChange={(e)=>{
                                            this.setState({ControllerGroups:e.target.value})
                                        }}
                                        input={<OutlinedInput label="Группы" />}
                                        renderValue={(selected)=>(
                                            <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5,width:"100px" }}>
                                            {selected.map((element) =>(
                                                <Chip label={element.description} key={element.id}/>  
                                            )    
                                            )}
                                            </Box>
                                        )}>
                                            {this.state.allowedControllerGroup?
                                            this.state.allowedControllerGroup.map((e)=>(
                                                <MenuItem
                                                    key={e.id}
                                                    value={e}
                                                >
                                                    {e.description}
                                                </MenuItem>
                                            ))
                                            :<MenuItem>Нет доступных групп</MenuItem>
                                            }
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