import axios from 'axios'
import React from 'react'
import { BaseUrl } from '../../App'

import { AgGridReact } from '@ag-grid-community/react';

import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';
import PopUpWindow from '../AdminPanel/PopUpWindow';
import { Button, Grid, Typography,Select, MenuItem, TextField } from '@mui/material';

import * as SignalR from "@microsoft/signalr"
import { AgChartsReact } from 'ag-charts-react';
import { AgChart, time } from 'ag-charts-community';



class ValueChart extends React.Component
{
  constructor(props) {
    super(props);

    console.log(props);

    this.chartRef = React.createRef();
    this.state = {
      updating:false,
      options: {
        autoSize: true,
        data: [],
        series: [

          {
            xKey: 'time',
            yKey: 'value',
          },
        ],
        axes: [
          {
            type: 'time',
            position: 'bottom',
            nice: false,
            tick: {
              interval: time.second.every(5),
            },
            label:{
              format:'%H:%M:%S',
            }
          },
          {
            type: 'number',
            position: 'left',
            label: {
              format: '#{f}',
            },
          },
        ],
        title: {
          text: '',
        },
        legend: {
          enabled: false,
        },
      },
    };
  }

  update = () => {
    let option = this.state.options;

    let tmp = this.props.context.GetValue(this.props.data.id);

    if(tmp==undefined)
      return;
    else if(option.data.length<=0)
    {
      tmp.time = new Date(tmp.time).getTime();
      option.data.push(tmp);
    }
    else if(option.data[option.data.length-1].time == tmp.time)
      return
    else
    {
      tmp.time = new Date(tmp.time).getTime();
      option.data.push(tmp);
    }
    if(option.data.length>60)
      option.data.shift();
    this.setState({options:option});
  };

  startUpdates = () => {
    if (this.state.updating) {
      return;
    }
    this.setState({updating:true});
    //@ts-ignore
    this.update();
    //@ts-ignore
    setInterval(this.update, 500);
  }


  componentDidMount() {}

  render() {
    return (
      <div  style={{height:'500px',marginBottom:'5em'}}>
        <AgChartsReact ref={this.chartRef} getRowId={this.props.data.ip+"/"+this.props.data.OutputId} options={this.state.options} onChartReady={(e)=>{this.startUpdates()}}/>
      </div>
    );
  }
}

class Render extends React.Component{
  constructor(props)
  {
    super(props)
    console.log(props);
  }

  render()
  {
    return(
      <Typography>
        {this.props.value}
      </Typography>
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
    let value = await this.props.context.GetEnum(this.props.data.version);
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

class RenderValue extends React.Component 
{

  constructor(props)
  {
    super(props)
  }

  render()
  {
    return(
      <Typography>
        {this.props.value}

      </Typography>
    )
  }
}

export default class UserValue extends React.Component{

  IntToIPV4(int)
  {
      let part1=int&255;
      let part2=(int>>8)&255;
      let part3=(int>>16)&255;
      let part4=(int>>24)&255;
      return part4.toString()+"."+part3.toString()+"."+part2.toString()+"."+part1.toString();
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

  AddCharts(value)
  {
    console.log(value);
    return (
    <Button onClick={(e)=>{ 
      let data={};
      data.labels=value.description;
      data.id={
        ip:value.ip,
        OutputId:value.OutputId,
      }
      this.setState({chart:data})}}>

      Вывести график
    </Button>)
  }

  ListenValue(api)
  {

  }
    constructor(props)
    {
        super(props)
        this.AddCharts= this.AddCharts.bind(this);
        this.ListenValue= this.ListenValue.bind(this);
        this.ChangeState = this.ChangeState.bind(this);


        this.state={
          HubConnection:null,
          ValueCharts:null,
          Request:null,
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
                  'PopUp':[this.AddCharts]
              },},
              {
                field: 'ID',
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
                headerName:"Название выхода",
                cellRenderer:Render
              },
              {
                field:"state",
                headerName:"Состояние выхода",
                cellRenderer:Render,
              },
              {
                field: 'value',
                headerName:"Значение выхода",
                cellRenderer:RenderValue
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
          chartId:undefined,
          ChartApi:undefined,
          chart:undefined,

          ChartId:undefined,
          chartValue:undefined,
        }

        

        this.state.Request= axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })

        this.state.HubConnection = new SignalR.HubConnectionBuilder()
          .withUrl(BaseUrl+"/value")
          .build();
        
        this.state.HubConnection.on("Receive",(value)=>{



          if(this.gridApi!=undefined && this.gridApi.destroyCalled==false)
          {
            let obj = JSON.parse(value);
            let RowApi = this.gridApi.getRowNode(obj.controllerAddress+"/"+obj.controllerOutputId);
            RowApi.setDataValue('value',obj.value)
            
            if(this.state.ChartId!=undefined && this.state.ChartId==obj.controllerAddress+"/"+obj.controllerOutputId)
            {
              let tmp={
                value:obj.value,
                time:obj.DateTime
              }
              this.setState({chartValue:tmp});
            }

          }

        })

        this.state.HubConnection.start();

        this.GetAllowedState= this.GetAllowedState.bind(this);
        this.GetValue=this.GetValue.bind(this);
    }


    GetValue(id)
    {

      if(this.state.ChartId==undefined || this.state.ChartId!=(id.ip+"/"+id.OutputId))
        this.setState({ChartId:id.ip+"/"+id.OutputId});
      else
      {
        return this.state.chartValue;
      }

    }


    async GetAllowedState(version)
    {
      let response = await this.state.Request.
        get("/api/ControllerState/GetAllowedControllerState?"+"Name="+version.name+"&Version="+version.version);
      
      switch(response.status)
      {
        case 200:{
          
          return response.data.value;
          break;
        }
      }
    }

    async GetAllowedOutputs()
    {
      let responce = await this.state.Request.get("/api/Value/AllowedOutput");

      console.log(responce);
      switch(responce.status)
      {
        case 200:
          {

            let data = []
            responce.data.value.forEach((e)=>{
              console.log(e);
              let tmp ={}
              tmp.ID=this.IntToIPV4(e.controllerAddress);
              tmp.name= e.name;
              tmp.description= e.description;
              tmp.time = undefined;
              tmp.state = e.outputState.description;
              tmp.id={
                ip:e.controllerAddress,
                OutputId:e.id,
                description:e.description,
              }
              data.push(tmp);
            })
            this.setState({readyData:data})
            break;
          }
      }
    }

    onGridReady = (params) => {
      //this.setState({gridApi:params.api});
      this.gridApi = params.api;
      this.gridColumnApi = params.columnApi;
    };

    
    componentDidMount()
    {
      //this.GetValues();
    }

    render()
    {
        console.log("value");
      return( <div>
        <Grid container height={"100%"} width={"100%"}>
        <Grid item height={"100%"} width={"100%"}>
          <div className="ag-theme-alpine" style={{ minHeight: 400,height:400, width: "100%" }}>
            <AgGridReact
            columnDefs={this.state.columnDefs}
            defaultColDef={this.state.defaultColDef}
            autoGroupColumnDef={this.state.autoGroupColumnDef}
            onGridReady={e=>{ this.GetAllowedOutputs();this.onGridReady(e)}}
            animateRows={true}
            getRowId={(x)=>{return x.data.id.ip+"/"+x.data.id.OutputId}}
            context={{
              'GetEnum':this.GetAllowedState,
              'ChangeSelect':this.ChangeState,
          }}
            rowData={this.state.readyData}
            />
          </div>
        </Grid>
        <Grid item height={"100%"} width={"100%"}>
          {this.state.chart? <ValueChart data={this.state.chart} context={{"GetValue":this.GetValue}} />: null}
        </Grid>
        </Grid>
      </div>);
    }
}