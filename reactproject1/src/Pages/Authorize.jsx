import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, Paper,Card, Grid, Typography, FormControl, InputLabel, OutlinedInput, InputAdornment, IconButton } from "@mui/material";
import { width } from "@mui/system";
import axios from "axios";
import React from "react";
import { BaseUrl } from "../App";
import { Navigate, redirect, useNavigate } from "react-router";




export default class Aythorize extends React.Component
{
    
    constructor(props)
    {
        super(props);
        this.state={
            password:null,
            showPassword:false,
            nickName:"",
            password:"",
            request:null,
            redirect:false,
            RedirectPath:null,
        }

        this.state.request = axios.create({
            baseURL:BaseUrl,
            withCredentials:true,
        })
        // const requestInterceptor = (request) => {
        //     request.withCredentials = true;
        //     return request;
        // };
        // this.state.request.interceptors.request.use(request => requestInterceptor(request));
    }


    async SendLoginAndPassword(e)
    {
        e.preventDefault();
        this.state.request.post("api/Authorization/Login",{LogIn:this.state.nickName ? this.state.nickName : null,Password:this.state.password ?this.state.password  : null}).
            then((e)=>{
                console.log(e);
                switch(e.status)
                {
                case 200:
                    {
                        this.setState({isAuthorization:true})
                       
                        if(e.data.value=='Admin')
                        {
                            this.setState({redirect:true,RedirectPath:"/Admin"})    

                        }
                        else if(e.data.value=='User')
                        {
                            this.setState({redirect:true,RedirectPath:"/User"})  
                            let navigate = new useNavigate();
                            navigate(this.state.RedirectPath,{replace:true});
                        } 
                        
                        break;
                    }
                case 401:
                default:
                    {
                        console.log("errore");
                        break;
                    }
                }
            });
    }


    redirect(){
        return <Navigate to ={this.state.RedirectPath}/>

    }
    componentDidUpdate()
    {
        console.log("component Update");
    }
    render()
    {
        return(
            <Paper sx={{
                display:"block",
                width:'100%',
                height:'100%',
            }}>
                <Card sx={{display:'flex',width:'400px',height:'100%'}}>
                    <Grid                 
                        display={'flex'}   
                        container
                        direction="column"
                        justifyContent="flex-start"
                        alignItems="center">
                            <Grid item marginTop={"10px"}>
                                <Typography>Авторизация</Typography>
                            </Grid>
                            <Grid item>
                                <FormControl>
                                    <InputLabel htmlFor="Name">Имя пользователя</InputLabel>
                                    <OutlinedInput
                                        label="NickName"
                                        id="NickNameid"
                                        onChange={(e)=>this.setState({'nickName': e.target.value})}>
                                    </OutlinedInput>
                                </FormControl>
                                <FormControl>
                                    <InputLabel htmlFor="Password">Пароль</InputLabel>
                                    <OutlinedInput
                                        label="Password"
                                        value={this.state.password}
                                        type={this.state.showPassword ? 'text' : 'password'}
                                        onChange={(e)=>this.setState({'password':e.target.value})}
                                        id="Passwordid"
                                        endAdornment={ 
                                            <InputAdornment position="end">
                                                <IconButton
                                                aria-label="toggle password visibility"
                                                onClick={(e)=>this.setState({showPassword:!this.state.showPassword})}
                                                 //onMouseDown={handleMouseDownPassword}
                                                edge="end"
                                                >
                                                    {this.state.showPassword ? <VisibilityOff /> : <Visibility />}
                                                </IconButton>
                                             </InputAdornment>}
                                    >
                                    </OutlinedInput>
                                </FormControl>
                            </Grid>
                            <Grid item>
                                <Button size="medium" onClick={(e)=> this.SendLoginAndPassword(e)}>
                                        Авторизация</Button>
                            </Grid>
                    </Grid>
                </Card>
                {this.state.redirect === true ? this.redirect() :<div> </div> }
            </Paper>
        )
    }
}