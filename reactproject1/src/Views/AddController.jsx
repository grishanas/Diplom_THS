import React from "react";
import axios from 'axios';
import {Input,Box,Button, Paper, FormControl, InputLabel, Card, Select} from "@mui/material"
import { height, width } from "@mui/system";

export default class AddController extends React.Component
{
    constructor(props)
    {
        super(props);
       
    }

    AddAdress()
    {
        if (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(this.state.ip))
        {

        }

    }

    render()
    {
        return(
            <Paper sx={{ marginTop:"10px", width:"100%",height:"200px" }}>
                <Card sx={{display:"flex",flexDirection:"column",justifyContent:"center",marginBottom:"20px"}} >
                    <FormControl>
                        <InputLabel>Введите ip адресс</InputLabel>
                        <Input onChange={(e)=>{this.setState({ip:e.target.value})}}></Input>
                    </FormControl>
                    <Button onClick={(e)=>{this.AddAdress()}}>
                        добавить
                    </Button>
                </Card>

                <Card sx={{display:"flex",flexDirection:"column",justifyContent:"center"}} >
                    <Select>

                    </Select>
                    <FormControl>
                        <InputLabel>Введите ip адресс</InputLabel>
                        <Input onChange={(e)=>{this.setState({ip:e.target.value})}}></Input>
                    </FormControl>
                    <Button onClick={(e)=>{this.AddAdress()}}>
                        добавить
                    </Button>
                </Card>


            </Paper>
        );
    }
}