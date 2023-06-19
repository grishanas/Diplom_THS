import react from 'react'
import { Grid,Paper } from "@mui/material";
import {Link } from 'react-router-dom';

export const Blog = (props)=>{


    return(
            <div className='container'>
                <div className='Blog'> 
                <h3>
                    {props ? props.data.title:""}
                </h3>
                <Grid xs={12} container className='BlogGrid' spacing={3}>
                    {props.data ? props.data.Blogs.map((value)=>(
                        <Grid className='BlogGrid-item' key={value.id} item>
                            <Link>
                                

                            </Link>
                        </Grid>
                    )):<p></p>}

                </Grid>


            </div>
        </div>
    );
}