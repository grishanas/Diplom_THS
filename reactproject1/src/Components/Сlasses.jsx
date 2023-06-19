import * as React from 'react';
import { Grid,Paper,Box } from "@mui/material";
import {Link } from 'react-router-dom';
import axios from 'axios';
import { baseURL } from '../App';


export class Classes extends React.Component{

    constructor(props)
    {
        super(props)

        this.state={
            request:axios.create({
                baseURL:baseURL,
                headers:{ 'Content-Type': 'application/json' },
            })
        }
    }


    async RequestClasses()
    {

    }

    componentDidMount()
    {

    }


    render()
    {
        return(
            <Box className='container'>
                <Box className='Classes'>
                    <Box className='ClassesDescript'>
                        <h1>{this.props.classes?this.props.classes.title:"Load"}</h1>
                    </Box>
                </Box>
                <Grid container className='ClassesGrid' spacing={2}>
                    {this.state.classList?this.state.classList.map((value)=>(
                        <Grid item>
                            <Link/>
                        </Grid>
                    )):null}

                </Grid>
                

            </Box>
        )
    }
}



/*export const  Classes=(props)=>
{

       return(
           <div className='container'>
               <div className='Classes'>
               <div className='ClassesDescript'>
               <h1>
                   {props.data ? <h1>{props.data.title}</h1>:<h1>что-то типо наших курсов</h1>}
               </h1>
               <p>
                   {props.data ? props.data.description :''}
               </p>
                </div>
            <Grid xs={12} container className='ClassesGrid' spacing={2}>
                {props.data ? props.data.Classes.map((value)=>(
                    <Grid className='ClassGrid-item' key={value.id} item>
                        
                        <Link to ={{pathname:  `Classes/${value.id.toString()}`}}>
                        <Paper elevation={1} style={{height:200,width:300}}>

                            
                        <div className='Class-Content'>
                        <img src={value.photo} className="Class-img"/>
                        <div className='Class-Content-Text'>
                        <h3 className='Class-title'> {value.title}</h3>
                        <p className='Class-descript'>{value.description}</p>
                        </div>
                        </div>
                            

                        </Paper>
                        </Link>
                    
                </Grid>
                )):<p>Load Classes</p>}
            </Grid>
            </div>
            </div>
            );   
}*/