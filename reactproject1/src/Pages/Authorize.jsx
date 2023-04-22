import { Visibility, VisibilityOff } from "@mui/icons-material";
import { Button, Paper,Card, Grid, Typography, FormControl, InputLabel, OutlinedInput, InputAdornment, IconButton } from "@mui/material";
import { width } from "@mui/system";
import axios from "axios";
import React from "react";
import { BaseUrl } from "../App";


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

        }

        this.state.request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        })

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
                        console.log(200);
                        //localStorage.setItem("RefreshToken",e.data.RefreshToken);
                        
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
                                <Button size="medium" onClick={e=>this.SendLoginAndPassword(e)}>Авторизация</Button>
                            </Grid>
                    </Grid>
                </Card>
            </Paper>
        )
    }
}