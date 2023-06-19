import {Box, Button, Card, Dialog, DialogActions, DialogTitle, FormControl, Grid, IconButton, Input, InputBase, InputLabel, MenuItem, OutlinedInput, Paper, Select, TextField, Tooltip, Typography } from '@mui/material'
import { arrayIncludes } from '@mui/x-date-pickers/internals/utils/utils';
import React from 'react'
import { CourseProgramms } from '../HelpComponents/CourseProgramm';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';

const allowedLangs=[
    {'key':'Rus','value':'русский'},
    {'key':'b','value':'b'},
    {'key':'c','value':'c'},
    {'key':'d','value':'d'}
]


class CourseProgramm extends React.Component{

    constructor(props)
    {
        super(props)
        this.state={
            course:new CourseProgramms(),
            lang:undefined,
            courseProgramm:undefined,
            lesons:undefined, 
            openAlertDialog:false,     

        }
    }

    addCourseProgramm()
    {
        this.state.courseProgramm.addCourseProgramm();
        this.setState({lesons:this.state.courseProgramm.getLesons()});
    }

    setLang(e)
    {
        this.setState({lang:e.target.value});
        this.setState({courseProgramm:this.state.course.setLang(e.target.value.key)});
        this.setState({lesons:this.state.course.setLang(e.target.value.key).getLesons()})
        
    }

    render()
    {
        console.log(this)

        return (
            <Paper style={{width:'100%'}}>
                <Grid container
                    direction='column'
                    spacing={2}>
                <Grid item container
                    justifyContent="center"
                    direction='column'
                    alignItems="center"
                    spacing={2}> 
                    <Grid item>
                    <FormControl style={{width:300}}>
                        <Select
                        displayEmpty={(this.state.lang==undefined)?true:false}
                         renderValue={(selected)=>{

                            if(this.state.lang===undefined)
                            {
                                selected=undefined;
                            }
                            
                            if(selected==undefined)
                            {
                                return (<em>placeholder</em>)
                            }
                           
                            return selected.value
                            }}
                            value={this.state.lang}
                            
                            onChange={(e)=>this.setLang(e)}>
                            {allowedLangs.map((lang)=>(
                            <MenuItem
                            key={lang.key}
                            value={lang}
                            input={<OutlinedInput />}
                            >
                            {lang.value}
                        </MenuItem>
                    ))}
                        </Select>
                    </FormControl>
                    </Grid>
                    <Grid item>
                        <TextField
                            style={{width:300}}
                            placeholder='input some text'
                        >

                        </TextField>
                    </Grid>
                </Grid>

               
                {this.state.courseProgramm?
                        <Grid item container spacing={4} direction="row">
                        {(this.state.lesons!=undefined)?this.state.lesons.map((item,index)=>{
                            return (
                            <Grid item justifySelf='center'>
                                <Paper
                                    style={{width:300,minHeight:200}}>
                                        <Grid container  direction='column' alignItems='center'>
                                            <Grid item marginBottom={4}>
                                                <TextField  variant='standard' size='medium' multiline  style={{minWidth:200}} placeholder="dsa"></TextField>
                                            </Grid>
                                            <Grid item container direction='column' alignItems='center' justifyContent='center'>
                                                {item.points.map((point,pointIndex)=>(
                                                    <Grid item container direction='row' justifyContent='space-evenly' >
                                                        <Grid item>
                                                            <Typography>{pointIndex+1}</Typography>
                                                        </Grid>                                                       
                                                        <Grid item>
                                                        <TextField      
                                                        variant='standard'
                                                        value={point.point} multiline rowsMax={2} style={{minWidth:160}}  placeholder="dsa"></TextField>
                                                        </Grid>
                                                        <Grid item>
                                                            <IconButton onClick={(e)=>this.setState({lesons:this.state.courseProgramm.deletePoint({lesonid:index,pointid:pointIndex})})}>
                                                                <DeleteIcon/>
                                                            </IconButton>
                                                        </Grid>
                                                    </Grid>

                                                ))}
          
                                                <Grid item>
                                                    <IconButton onClick={(e)=>
                                                        this.setState({Lesons:this.state.courseProgramm.setCourseProgrammPoint({lesonid:index})})}>
                                                        <AddIcon/>
                                                    </IconButton>
                                                </Grid>
                                            </Grid>
                                            <Grid item>
                                                <Tooltip title="delete Lesson">
                                                <IconButton onClick={(e)=>this.setState({openAlertDialog:true})}>
                                                    <DeleteIcon/>
                                                </IconButton>
                                                </Tooltip>
                                                <Dialog
                                                    open={this.state.openAlertDialog}
                                                    onClose={(e)=>{this.setState({openAlertDialog:false})}}
                                                >
                                                    <DialogTitle>
                                                        someText
                                                    </DialogTitle>
                                                    <DialogActions>
                                                        <Button onClick={(e)=>this.setState({lesons:this.state.courseProgramm.deleteLeson({lesonid:index}),openAlertDialog:false})}>
                                                            Delete
                                                        </Button>
                                                        <Button autoFocus={true} onClick={(e)=>this.setState({openAlertDialog:false})}>
                                                            don't Delete
                                                        </Button>
                                                    </DialogActions>

                                                </Dialog>
                                            </Grid>
                                        </Grid>
                                </Paper>
                                </Grid>)
                            
                            
                        }):null}
                        <Grid item>
                            
                            <IconButton onClick={(e)=>this.addCourseProgramm()}>
                            <Tooltip title="Add">
                            <Paper style={{width:300,height:400,display:'flex', alignItems:'center',justifyContent:'center'}}>
                                <AddIcon/>
                            </Paper>
                            </Tooltip>
                            </IconButton>
                            
                        </Grid>
                        </Grid>
                    :null}
                </Grid>
                <Button onClick={(e)=>{console.log(this.state)}}>
                                press
                </Button>
            </Paper>
        )
    }

}

export default CourseProgramm