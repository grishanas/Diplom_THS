import * as React from 'react';
import { Grid,Paper } from "@mui/material";


export const Educators = (props)=>
{
       return(
        <div className='Educators'>
           <div className='container'>
               <div className='EducatorsDescript'>
                    <h3>
                        {props.data ? props.data.title : "Преподаватели грузятся"}
                    </h3>
                    <p>
                        {props.data ? props.data.description : "Какое-то описание"}
                    </p>
               </div>
               <Grid container className='EducatorsGrid' xs={12} spacing={4}>
                   {props.data ? props.data.Educators.map((value)=>(
                       <Grid className='EducatorsGrid-item' key={value.id} item>
                           <Paper elevation={1} style={{width:300}} className='EducatorsGrid-item-Paper'>
                                <div className='EducatorsGrid-content'>
                                    <img src={value.photo}/>
                                    <div className='EducatorsGrid-content-text'>
                                    <h3>
                                        {value.title}
                                    </h3>
                                    <p>
                                        {value.description}
                                    </p>
                                    </div>
                                </div>
                           </Paper>
                       </Grid>
                   )) :<p>dsa</p> }
               </Grid>
           </div>
           </div>
       );
}