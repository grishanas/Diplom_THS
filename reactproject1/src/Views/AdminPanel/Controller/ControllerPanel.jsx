import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button, MenuItem, Chip, OutlinedInput } from "@mui/material";
import React, { Component } from "react";
import PopUpWindow from "../PopUpWindow";
import axios from "axios";
import { BaseUrl } from "../../../App";
import QueryPanel from "./QueryPanel";
import { Navigate, Route, useNavigate } from "react-router"
import { Routes} from "react-router-dom"
import Tippy from "@tippyjs/react";


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

class MultipleRender extends React.Component{

    constructor(props)
    {
        super(props);
        console.log(props);
    }

    render()
    {
        return this.props.value !=undefined && this.props.value.length>0 ? 
        this.props.value.map((e)=>{
            return <Button>
                {e}
            </Button>
        }):<Typography>Нет групп</Typography>
    }
}

class AddRoleToGroup extends React.Component
{
    constructor(props)
    {
        super(props);
        this._text="Добавить группу";
        this.state={Request:null};
        this.state={visible:false,hide:true,AvalibelRoles:null}
        if(this.props.value!==undefined)
            this.state={hide:false};   
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true
        });
        
    }

    async Add()
    {
        // this.state.Request.post("/api/User/AddRoleToUser",{
        //     user:this.props.id,
        //     userRole:role.id
        // }).then((e)=>{console.log(e);this.props.RefreshGrid()});  
    }


    AddRoleToGroupContent()
    {
        // let tmp = this.props.Request.get("/api/Controller/GetController/"+this.props.id);
        // let avalibleRoles= this.props.GetRoles();
        // let count=0;
        // return <div>
        //     {avalibleRoles? 
        //         avalibleRoles.map((element)=>{
        //             let i=0;
        //             count++;
        //             for(;i<tmp.userRoles.length;i++)
        //             {
        //                 if(element.id == tmp.userRoles[i].id)
        //                 {
        //                     return null;
        //                 }
        //             }
        //             return <button onClick={(e)=>this.addRole(element)}>{element.description}</button>
        //         })
        //     :
        //     <div>
        //         {count>0?<p> нет доступных ролей</p>:<p>Загрузка ролей</p>}
        //     </div>}
        // </div>
    }

    render()
    {
        return <div>
            <Tippy>
                content={this.AddRoleToGroupContent()}
                visible = {this.state.visible}
                onClickOutside={(e)=>this.setState({visible:false})}
                allowHTML={true}
                arrow={false}
                appendTo={document.body}
                interactive={true}
                placement="right"
            <Button>
                {this._text}
                </Button>
            </Tippy>
        </div> 
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
        return <QeuryRoute id={id} context={context}/>
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
                    'PopUp':[this.AddControllerGroup,this.GetControllerQuery]
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
                },
                {
                    field:"state",
                    headerName:"Состояние",
                    cellRenderer:Render,
                },
                {
                    field:"ControllerGroups",
                    headerName:"Группы контроллеров",
                    cellRenderer:MultipleRender
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

    componentDidUpdate()
    {
        console.log(this.state.ControllerName)
    }

    async GetControllerGroup()
    {
        let responce = await this.state.Request.get("/api/ControllerGroup/GetControllerGroup");

        console.log(responce);
        switch(responce.status)
        {
            case 200:{

                let data =[];
                responce.data.value.forEach((e)=>{
                    let tmp={};
                    tmp.id= e.id;
                    tmp.description = e.description;
                })

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
                    tmp.description = e.description;
                    data.push(tmp);
                })
                this.setState({controllerStates:data});
                break;
            }
        }
        this.GetControllerGroup();
    }
    
    renderClean()
    {
        return 
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
                                context={this.props}
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
                                                key={e.description}
                                                value={e.description}
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