import * as React from "react";
import { Box, fontSize } from "@mui/system";
import { Button, Card, Dialog, DialogContent, DialogContentText, DialogTitle, IconButton, Input, MenuItem, OutlinedInput, Paper, Select, TextField, Typography } from "@mui/material";
import AddPhotoAlternateSharpIcon from '@mui/icons-material/AddPhotoAlternateSharp';
import axios from "axios";
import AddPhoto from './AddPhoto'
import PhotoState from '../HelpComponents/PhotoState'
import addTeacher from "../HelpComponents/AddTeacher";

const baseURL='http://localhost:3001';

class AddTeacher extends React.Component
{
    constructor(props)
    {
        console.log(props);
        super(props);
        let Teacher;
        
        if(props.hasOwnProperty('teacher'))
        {
            Teacher= props.teacher;
        }
        else
        {
            Teacher= new addTeacher();
        }

        this.state={
            photo: new PhotoState(),
            lang: 'Placeholder',
            personalData:'',
            teacherDescripion:'',
            teacher:Teacher,
            isTextFieldEnable:false,
        }

    }
    
    changeTextField(name,e){
        console.log(name);
        console.log(e.target.value)
        this.setState({[name]:e.target.value})
    }


    changeLanguage(e)
    {
        this.setState({isTextFieldEnable:true});
        if(this.state.lang!=undefined)
        {
            let tmp={'lang':this.state.lang,'personalData':this.state.personalData,'teacherDescripion':this.state.teacherDescripion};
            this.state.teacher.setState(tmp);

        }
        let TeacherData = this.state.teacher.getTeacherInformation(e.target.value.key);
        this.setState(TeacherData.getState());
        this.setState({lang:e.target.value});


    }


    sendRequest()
    {

        /*
            пересылка запроса дочернему элементу
        */

    }

    sendTeacher()
    {
        if(this.state.lang!=undefined)
        {
            let tmp={'lang':this.state.lang,'personalData':this.state.personalData,'teacherDescripion':this.state.teacherDescripion};
            this.state.teacher.setState(tmp);

            let formData= new FormData();

            this.props.request.post('/Admin/PostTeacher',{}).then((e)=>{
                console.log(e);
            })
        }
    }

    componentDidUpdate(prevProps)
    {
        if(prevProps.lang!==this.props.lang)
        {
            console.log(this.props);
        }
    }

    resetState()
    {

        this.setState({
            photo: new PhotoState(),
            lang: undefined,
            personalData:'',
            teacherDescripion:'',
            teacher: new addTeacher(),
            isTextFieldEnable:false,
        })
    }


    render()
    {
        return(
            <Card style={{
                display:'flex',
                justifyContent:'center',
                flexDirection:'column',
            }}
            >
                <Typography style={{margin:'auto'}}>{(this.props.HeaderAddTeacher)?this.props.HeaderAddTeacher:"Добавление преподавателя"}</Typography>
                <AddPhoto state={this.state.photo} dataTooltip="sometext"/>
                <Box style={{display:'flex',margin:'auto', flexDirection:'column'}}>
                
                <Select sx={{m:1,mt:3,width:300}} style={{margin:'auto',marginTop:10}}
                    displayEmpty
                    value={this.state.lang}
                    onChange={(e)=>this.changeLanguage(e)}

                    input={<OutlinedInput />}>
                        <MenuItem disabled value='Placeholder'>
                            <em>Placeholer</em>
                        </MenuItem>
                    {this.props.lang?this.props.lang.map((lang)=>(
                        <MenuItem
                            key={lang.key}
                            value={lang.key}>
                            {lang.value}
                        </MenuItem>
                    )):null}
                </Select>
                <TextField
                disabled={!this.state.isTextFieldEnable}
                placeholder={this.props.lang?this.props.lang.persanalData:"Персональные данные"}
                value={this.state.personalData}
                onChange={(e)=>this.changeTextField('personalData',e)}
                >
                </TextField>
                <TextField
                disabled={!this.state.isTextFieldEnable}
                multiline
                rows={4}
                placeholder={this.props.lang?this.props.lang.teacherDescripion:"Описание"}
                variant="standard"
                value={this.state.teacherDescripion}
                onChange={(e)=>this.changeTextField('teacherDescripion',e)}
                />
                </Box>
                <Box margin={'auto'}>
                <Button onClick={(e)=>{this.sendTeacher()}}>
                    добавить
                </Button>
                <Button onClick={(e)=>{this.resetState()}}>
                    очистить
                </Button>
                </Box>
            </Card>
        )
    }
}

export default AddTeacher;
