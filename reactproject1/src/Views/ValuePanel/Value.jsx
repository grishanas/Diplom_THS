import axios from 'axios'
import React from 'react'
import { BaseUrl } from '../../App'
import { Line } from "react-chartjs-2";

import Chart from "chart.js/auto";
import { CategoryScale } from "chart.js"

import { AgGridReact } from '@ag-grid-community/react';
import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';
import PopUpWindow from '../AdminPanel/PopUpWindow';
import { Button, Grid, Typography } from '@mui/material';


const chartData = { 
    labels: ['Red', 'Orange', 'Blue'],
// datasets is an array of objects where each object represents a set of data to display corresponding to the labels above. for brevity, we'll keep it at one object
datasets: [
    {
      label: 'Popularity of colours',
      data: [55, 23, 96],
      backgroundColor: [
        'rgba(255, 255, 255, 0.6)',
        'rgba(255, 255, 255, 0.6)',
        'rgba(255, 255, 255, 0.6)',
      ],
      borderWidth: 1,
    }
]}

class ValueChart extends React.Component{

  constructor(props)
  {
    super(props)
  }

  render()
  {
    return (<div>
      <h2 style={{ textAlign: "center" }}>Line Chart</h2>
        <Line
          data={chartData}
          options={{
            plugins: {
              title: {
                display: true,
                text: "Users Gained between 2016-2020"
              },
              legend: {
                display: false
              }
            }
          }}
        />
    </div>)
  }
}

class Render extends React.Component{
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

class ValueRender extends React.Component 
{

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

Chart.register(CategoryScale);

export default class ValuePanel extends React.Component{

  IntToIPV4(int)
  {
      let part1=int&255;
      let part2=(int>>8)&255;
      let part3=(int>>16)&255;
      let part4=(int>>24)&255;
      return part4.toString()+"."+part3.toString()+"."+part2.toString()+"."+part1.toString();
  }

  async GetValues()
  {
    let response = await this.state.Request.get("/api/Value/StartListen");
    console.log(response);
    let streamReader= response.body.getReader();

    console.log(streamReader);
    while(true)
    {
      const {done, value} = await streamReader.read();
      if (done) {
        break;
      }
      
      console.log(value)
    }


  }

  async GetValue(id)
  {

  }

  AddCharts(value)
  {
    console.log(value)
    console.log(this);
    return (
    <Button onClick={(e)=>{this.GetValue(value); 
      let data=this.state.chart; 
      data.labels=value.description;
      this.setState({chart:data})}}>

      Вывести график
    </Button>)
  }

  DeleteChart()
  {

  }

    constructor(props)
    {
        super(props)
        this.AddCharts= this.AddCharts.bind(this);
        this.DeleteChart = this.DeleteChart.bind(this);

        this.state={
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
                  'PopUp':[this.AddCharts,this.DeleteChart]
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
                field: 'Value',
                headerName:"Значение выхода",
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
          chartId:undefined,
          chart:{
          labels: [],
          datasets: [
              {
                label: '',
                data: [],
                backgroundColor: [
                  'rgba(255, 255, 255, 0.6)',
                  'rgba(255, 255, 255, 0.6)',
                  'rgba(255, 255, 255, 0.6)',
                ],
                borderWidth: 1,
              }
          ]}
        }

        

        this.state.Request= axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })
    }

    async GetAllowedOutputs()
    {
      let responce = await this.state.Request.get("/api/Value/AllowedOutput");

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
              tmp.state = e.outputState.description
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

    componentDidMount()
    {
      this.GetValues();
    }

    render()
    {
      return( <div>
        <Grid item>
          <div className="ag-theme-alpine" style={{ minHeight: 400,height:400, width: "100%" }}>
            <AgGridReact
            columnDefs={this.state.columnDefs}
            defaultColDef={this.state.defaultColDef}
            autoGroupColumnDef={this.state.autoGroupColumnDef}
            onGridReady={e=>{ this.GetAllowedOutputs()}}
            animateRows={true}
            rowData={this.state.readyData}
            />
          </div>
        </Grid>
        <Grid item>
          {this.state.charts? <ValueChart data={this.state.chart}/>: null}
        </Grid>
      </div>);
    }
}