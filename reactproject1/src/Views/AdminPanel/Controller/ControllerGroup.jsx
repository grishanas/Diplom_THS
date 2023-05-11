import { Box, Button,Grid, Card, Dialog, DialogContent, FormControl, FormLabel, Typography, InputLabel, Input } from "@mui/material";
import PopUpWindow from "../PopUpWindow";
import React from "react";
import { BaseUrl } from "../../../App";
import axios from "axios";
import { AgGridReact } from "@ag-grid-community/react";

import '@ag-grid-community/styles/ag-grid.css';
import '@ag-grid-community/styles/ag-theme-alpine.css';


class RenderItem extends React.Component{

    constructor(props)
    {
        super(props);
    }

    render()
    {
        return <Typography>{this.props.value}</Typography>
    }
}

class DeleteControllerGroup extends React.Component{
    
    constructor(props)
    {        
        super(props);
        this.state={visible:false}
    }

    delete()
    {

        this.props.params.Request.delete("/api/ControllerState/Delete/"+this.props.id).
        then((e)=>{
            console.log(e);
        })
    }

    render()
    {
        return <div>
        <Button variant="text" onClick={(e)=>this.setState({visible:true})}>
            Удалить состояние
        </Button>
        <Dialog 
        open={this.state.visible}
        onClose={(e)=>this.setState({visible:false})}>
            <DialogContent> 
            <Box display={"flex"} flexDirection={"column"} >
                <Typography> Вы точно хотите удалить состояние ?</Typography>
                <Box flexDirection={"row"}>
                    <Button onClick={(e)=>{this.delete();this.setState({visible:false}) }}>Подтвердить</Button>
                    <Button onClick={(e)=>{this.setState({visible:false})}}>Отменить</Button>
                </Box>
            </Box>
            </DialogContent>
        </Dialog>
    </div>
    }

} 


export default class ControllerGroup extends React.Component
{
    deleteController(id,params)
    {
        return <DeleteControllerGroup id={id} params={params}/>
    }
    constructor(props)
    {
        
        super(props);
        this.state={
            columnDefs:[
                {
                    field:'RoleLength',
                    headerName:"Действие",
                    pinned: 'left',  
                    colid:"action",    
                    editable: false,
                    maxWidth: 150,
                    cellRenderer:PopUpWindow,
                    cellRendererParams:{
                        'PopUp':[this.deleteController],
                    },
                },
                { 
                    field: 'id',
                    headerName:"ID",
                    cellRenderer:RenderItem
                },
                {
                    field:'description',
                    headerName:'Описание',
                    cellRenderer:RenderItem
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

        this.deleteController= this.deleteController.bind(this);
        this.state.Request=null;
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        })
    }

    async GetControllerName()
    {
        this.state.Request.get("/api/ControllerGroup/GetAll").then((e)=>{
            switch(e.status)
            {
               
                case 200:{
                    let data =[];
                    e.data.value.forEach((element)=>{
                        let tmp = {};
                        tmp.id=element.id;
                        tmp.description= element.description;
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

    async SendNewName()
    {
        this.state.Request.post("/api/ControllerState/Add",{
            description:this.state.description,
        }).then((e)=>{
            this.setState({description:''});
            this.GetControllerName();
        })
    }

    render()
    {
        return <div>
            <Grid
                width="100%"
                container
                direction="column"
                justifyContent="center"
                alignItems="stretch"
            >
                <Grid item>
                    <Typography textAlign={"center"}>
                        Названия контрроллеров
                    </Typography>
                    <Grid item>
                        <div className="ag-theme-alpine" style={{ minHeight: 400,height:400, width: "100%" }}>
                            <AgGridReact
                                columnDefs={this.state.columnDefs}
                                defaultColDef={this.state.defaultColDef}
                                autoGroupColumnDef={this.state.autoGroupColumnDef}
                                animateRows={true}
                                rowData={this.state.readyData}
                                context={{'Request':this.state.Request}}
                                onGridReady={(e)=>{this.GetControllerName()}}
                            />
                        </div>
                    </Grid>
                    <Grid item>
                        <Card>
                            <Box  
                                display={"flex"} 
                                component="form"
                                flexDirection="column"
                                justifyContent="center"
                                alignItems="center"
                            >
                                <FormLabel>
                                    Добавить состояние
                                </FormLabel>
                                <FormControl>
                                <InputLabel>Описание состояния</InputLabel>
                                    <Input
                                        required
                                        value={this.state.description}
                                        onChange={(e)=>this.setState({description:e.target.value})}
                                    />

                                </FormControl>
                                <div>
                                    <FormControl>
                                        <Button onClick={(e)=>this.SendNewName()}> 
                                            Добавить
                                        </Button>
                                    </FormControl>
                                    <FormControl>
                                        <Button onClick={(e)=>{
                                            this.setState({description:""});
                                        }}>
                                            Отменить
                                        </Button>
                                    </FormControl>
                                </div>
                            </Box>
                        </Card>
                    </Grid>
                </Grid>

            </Grid>

        </div>
    }
}