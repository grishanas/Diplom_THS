import React from 'react'
import axios, { Axios } from 'axios'
import { Button, FormControl, Grid, InputBase, InputLabel, MenuItem, NativeSelect, OutlinedInput, Paper, Select, Typography } from '@mui/material';
import AddPhoto from './AddPhoto';
import LoadPhoto from '../HelpComponents/LoadPhoto';

const baseURL = 'http://localhost:3001';
const URL='/Admin/PutchTeacher';

class PutchPaper extends React.Component{

    constructor(props)
    {
        super(props)
        let tmp=new LoadPhoto();
        this.state=tmp.getState();
        //this.state = props.state.getState();
        //this.state.photo = props.state.loadFile;
        this.state.photo= tmp
        this.state.update=false;    
    }

    onCliclUpdate(e)
    {
        this.props.state.setState()
        this.props.state.sendRequest();
        this.state.update=false;   

    }

    changeLabel(name,e)
    {
        console.log(e);
        console.log(e.nativeEvent.inputType!='insertText');
        if(((this.state[name].length<40)||(e.nativeEvent.inputType!='insertText')))
        {
            this.setState({[name]:e.target.value});
            this.setState({update:true});
        }
    }

    render()
    {
        return (
        <Paper>
            <Grid container columnSpacing={2} >
                <Grid item>
                    <AddPhoto state={this.state.photo /* this.props.state */}/>
                </Grid>
                <Grid item xs={12} sm container>
                    <Grid item xs container direction="column" spacing={2}>
                        <Grid item>
                        <Grid item>
                        <FormControl variant="standard" sx={{ m: 1, minWidth: 120 }}>
                            <InputLabel>Choose lang</InputLabel>
                            <Select
                 
                            >    
                            <MenuItem value={10}>Ten</MenuItem>
                            <MenuItem value={20}>Twenty</MenuItem>
                            <MenuItem value={30}>Thirty</MenuItem>
                            </Select>
                            </FormControl>
                            
                          
                        </Grid>
                        <Grid item>
                            
                            <InputBase multiline maxRows={4} placeholder='some' value={this.state.description} onChange={(e)=>{this.changeLabel('description',e)}}/>
                        </Grid>
                        <Grid item>
                            <InputBase placeholder='ssd' value={this.state.personalData} onChange={(e)=>{this.changeLabel('personalData',e)}}/>
                        </Grid>

                        </Grid>
                        <Grid item xs container alignItems="flex-end">
                            <Button onClick={(e)=>{console.log(this.state)}}>
                                Remove
                            </Button>
                            <Button disabled={!this.state.update}>
                                Update
                            </Button>
                        </Grid>
                        
                    </Grid>
                </Grid>
            </Grid>
        </Paper>)
        
    }
}

class PutchTeacher extends React.Component{

    constructor(props)
    {
        super(props)
        this.state={
            loadPhoto:new LoadPhoto()
        }
        console.log(new LoadPhoto());
    }

    async requstTeacher(number)
    {
        const Requst = axios.create({
            baseURL: baseURL,
            headers:{ 'Content-Type': 'application/json' },
        });

        Requst.get(URL).then((e)=>{console.log(e)
        }).catch((e)=>{
            console.log(e);
        })
    }

    render()
    {
        this.requstTeacher(1);

    return(<Grid
        container
        direction="row"
        justifyContent="flex-start"
        alignItems="center"
        spacing={1}>
            {(this.state.loadPhotos)?
            this.state.loadPhotos.map((element)=>(
                <Grid item xs='auto'>
                    <PutchPaper state={element}/>
                </Grid> 
            ))
            :null
            }
            <Grid item xs='auto'>
                <PutchPaper state={new LoadPhoto()}/>
            </Grid>   
        </Grid>);
    }
}

export default PutchTeacher;