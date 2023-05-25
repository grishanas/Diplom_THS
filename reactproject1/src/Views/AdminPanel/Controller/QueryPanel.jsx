import React, { useRef } from "react";
import axios from "axios";
import { BaseUrl } from "../../../App";
import { AgGridReact } from "@ag-grid-community/react";
import { Card, Typography,Grid, Paper, Box, FormControl, FormLabel, InputLabel, Input, Select, Button, MenuItem, Chip, OutlinedInput, Autocomplete, TextField } from "@mui/material";
import PopUpWindow from "../PopUpWindow";
import QueryDialog from "../QueryDialog";


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



class SelectRender extends React.Component
{

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
    }

    async DeleteOutputGroup(Groups)
    {
        let diffrent = this.state.Groups.filter(i=>!Groups.includes(i))
        .concat(Groups.filter(i=>!this.state.Groups.includes(i)));
        console.log(diffrent);  
        let response =  await this.state.Request.delete("/api/ControllerOutput/DeleteControllerOutputFromGroup",{
            data:{
            ipAddress:this.props.data.id.controllerAddress,
            outputId:this.props.data.id.id,
            groupId:diffrent[0].id,
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


    async AddoutputToGroup(Groups)
    {
        let response =  await this.state.Request.post("/api/ControllerOutput/AddControllerOutputToGroup",
        {
            ipAddress:this.props.data.id.controllerAddress,
            outputId:this.props.data.id.id,
            groupId:Groups[Groups.length-1].id
        });

        switch(response.data.statusCode)
        {
            case 200:
            {
                this.setState({Groups:Groups});
                break;
            }
        }
    }

    componentDidUpdate()
    {
        this.UpdateHeight();
    }




    UpdateHeight()
    {

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
                    let groups= this.props.context.GetAllowedGroup();
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

class RenderSelect extends React.Component{

    constructor(props)
    {
      super(props)
      console.log(props);
      
      this.state={MenuItem:null,value:null}
      this.state.value= this.props.value;
    }
  
    async GetEnum()
    {
      let value = await this.props.context.GetEnum(this.props.data);
      this.setState({MenuItem:value});
    }
  
    async componentDidMount()
    {
      console.log(this.props);
      this.GetEnum();
    }
  
    render()
    {
      return(
        <TextField
        select
          margin='none'
          native
          variant="standard"
          onClick={(e)=>{this.props.context.ChangeSelect(this.props.data.id,e.target.dataset.value)}}
          value={this.props.value}
        >
          {this.state.MenuItem?this.state.MenuItem.map((e)=>(
            <MenuItem
              value={e.description}
              key={e.description}
            >
              {e.description}
            </MenuItem>
          )):null}
          
        </TextField >
      )
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

    async GetAllowedState(version)
    {
        let response = await this.state.Request.
        get("/api/ControllerState/GetAllowedOutputControllerState?"+"Name="+this.props.id.name+"&Version="+this.props.id.version);
      
      switch(response.status)
      {
        case 200:{
          this.setState({AllowedOutputState:response.data.value});
          return response.data.value;
          break;
        }
      }
    }

    async ChangeState(id,value)
    {
      console.log(id);
      let response = await this.state.Request.post("/api/ControllerOutput/SetOutputState",{
        outputId:id.OutputId,
        address:id.ip,
        stateDescription:value
      })
  
      console.log(response);
      switch(response.data.statusCode)
      {
        case 200:{
            let RowApi = this.gridApi.getRowNode(id.ip+"/"+id.OutputId);
            RowApi.setDataValue('state',value);
          break;
        }
      }
    }

    async sendNewGroup()
    {
        let responce = await this.state.Request.post("/api/ControllerGroup/AddOutputGroup/" ,this.state.NewGroup);

        switch(responce.data.statusCode)
        {
            case 200:{
                this.GetAllowedOutputGroups();
                break;
            }
        }
    }

    renderQueryDialog(id,context)
    {
        return <QueryDialog id={id} contex={context}/>
    }

    constructor(props)
    {
        console.log('Render Query');
        super(props);
        console.log(props);

        this.ChangeState=this.ChangeState.bind(this);
        this.GetAllowedState = this.GetAllowedState.bind(this);
        this.renderQueryDialog= this.renderQueryDialog.bind(this);
        

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
                    'PopUp':[this.renderQueryDialog]
                }},
                { field: 'ID',
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
                    field:"state",
                    headerName:"Состояние выхода",
                    autoHeight: true,
                    cellRenderer:RenderSelect,
                },
                {
                    field:"ControllerGroups",
                    headerName:"Группы выходов",
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
            groups:[],
            readyData:undefined,
            controllerName:undefined,
            NewGroup:undefined,
            AllowedOutputGroups:undefined,
            loading:false  
        }
        this.state.loading=this.state.AllowedOutputGroups===0;
        
       
        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            withCredentials:true,
        })

        this.GetGroups= this.GetGroups.bind(this);
    }

    GetGroups()
    {
        console.log('Groups');
        console.log(this.state.AllowedOutputGroups);
        return this.state.AllowedOutputGroups;
    }

    async GetAllowedOutputGroups()
    {

        let response = await this.state.Request.get("/api/ControllerGroup/GetOutputGroups");

        switch(response.data.statusCode)
        {
            case 200:{
                let data=[];
                response.data.value.forEach((item)=>{
                    let tmp ={};
                    tmp.id= item.id;
                    tmp.description = item.description;
                    data.push(tmp);
                })
                this.setState({AllowedOutputGroups:data});
                break;
            }
        }
    }

    IPV4toInt(ip)
    {
        
        let parts=ip.split(".");
        let res=0;

        console.log(parts.length);

        if(parts.length!=4)
            return null;
        res += parseInt(parts[0], 10) << 24;
        res += parseInt(parts[1], 10) << 16;
        res += parseInt(parts[2], 10) << 8;
        res += parseInt(parts[3], 10);

        console.log(res);
        return res;
    }

    async GetOutput()
    {
        this.GetAllowedOutputGroups()
        console.log(this.state);
        let id = this.IPV4toInt(this.props.id.id);
        console.log(id);
        let responce = await this.state.Request.get("/api/ControllerOutput/Outputs?address="+id);
        switch(responce.status)
        {
            case 200:
            {
                var data = []
                responce.data.value.forEach((e)=>{
                    console.log(e);
                    let tmp = {};
                    tmp.ID=e.id;
                    tmp.id={
                        controllerAddress:e.controllerAddress,
                        id:e.id
                    };
                    tmp.state= e.outputState.description;
                    tmp.name=e.name;
                    tmp.description = e.description;
                    tmp.ControllerGroups = [];
                    e.outputGroups.forEach((e)=>{
                        tmp.ControllerGroups.push({                        
                            id:e.id,
                            description:e.description})
                    });
                    data.push(tmp);
                })
                this.setState({readyData:data});
                break;
            }
        }
    }

    onGridReady = (params) => {
        //this.setState({gridApi:params.api});
        this.gridApi = params.api;
        this.gridColumnApi = params.columnApi;
    };

    async AddOutput()
    {

        let state= this.state.AllowedOutputState.find(x=>x.description===this.state.OutputState);
        console.log(state);
        let response = await this.state.Request.post("/api/ControllerOutput/Output",{
            id:0,
            controllerAddress:this.IPV4toInt(this.props.id.id),
            name:this.state.OutputName,
            description:this.state.OutputDescription,
            outputGroups:this.state.groups,
            outputStateId:state.id,
            outputState:state
        });

        console.log(response);
        switch(response.data.statusCode)
        {
            case 200:{
                this.GetOutput();
            }
        }

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
                    Выходы контроллера: {this.props.id?.id}
                </Typography>
                <Grid item>
                    <div className="ag-theme-alpine" style={{ height: 400, width: "100%" }}>
                        <AgGridReact
                            columnDefs={this.state.columnDefs}
                            defaultColDef={this.state.defaultColDef}
                            autoGroupColumnDef={this.state.autoGroupColumnDef}
                            animateRows={true}
                            rowData={this.state.readyData}
                            getRowId={(x)=>{return x.data.id.controllerAddress + "/"+ x.data.id.id}}
                            context={{"GetAllowedGroup" : this.GetGroups,'GridApi':this.gridApi,              
                            'GetEnum':this.GetAllowedState,
                            'ChangeSelect':this.ChangeState,
                        }}
                            onGridReady={(e)=>{this.GetOutput();this.onGridReady(e)}}
                        />
                    </div>
                 </Grid>
            </Grid>
            <Grid container flexDirection={"row"} justifyContent={"space-around"}>
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
                    Добавить новый выход
                </FormLabel>
                <FormControl>
                    <InputLabel>Название выхода</InputLabel>
                    <Input
                    required
                    value={this.state.OutputName}
                    onChange={(e)=>this.setState({OutputName:e.target.value})}
                    />
                </FormControl>
                <FormControl>
                    <InputLabel>Описание выхода</InputLabel>
                    <Input
                        required
                        value={this.state.OutputDescription}
                        onChange={(e)=>this.setState({OutputDescription:e.target.value})}
                    />
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }}>
                    <InputLabel>Состояние контроллера</InputLabel>
                    <Select
                        value={this.state.OutputState}
                        onChange={(e)=>{
                            this.setState({OutputState:e.target.value});
                        }}
                    >
                    {this.state.AllowedOutputState?
                        this.state.AllowedOutputState.map((e)=>(
                            <MenuItem
                                key={e.description}
                                value={e.description}
                            >
                                {e.description}
                            </MenuItem>
                        ))
                    :
                        <MenuItem>Ожидание состояния выхода контроллера</MenuItem>}
                    </Select>
                </FormControl>
                <FormControl sx={{ m: 1, minWidth: 200 }}>
                {this.state.AllowedOutputGroups?<Autocomplete
                        multiple
                        loading={this.state.loading}
                        onChange={(e,value,reson)=>{
                            this.setState({groups:value});
                        }}
                        isOptionEqualToValue={(option,value)=>option?.id?.toString()===value?.id?.toString()}
                        value={this.state.groups}
                        options={this.GetGroups()}
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
                    </Autocomplete>:null}
                </FormControl>
                <FormControl>
                    <Button onClick={(e)=>this.AddOutput()}>Подтвердить</Button>
                </FormControl>


            </Box>

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
                        Добавить группу выходов
                    </FormLabel>
                    <FormControl>
                        <InputLabel>Название группы</InputLabel>
                        <Input
                        required
                        value={this.state.NewGroup}
                        onChange={(e)=>this.setState({NewGroup:e.target.value})}
                        id="input-password"/>
                    </FormControl>
                    <FormControl>
                        <Button onClick={(e)=>this.sendNewGroup()}>Подтвердить</Button>
                    </FormControl>
                </Box>
            </Grid>
            </Grid>
        </Grid>
        </div>
    }
}