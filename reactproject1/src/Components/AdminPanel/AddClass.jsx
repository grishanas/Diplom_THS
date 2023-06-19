import AddAPhoto from '@mui/icons-material/AddAPhoto';
import { Grid, Typography } from '@mui/material';
import { Box } from '@mui/system'
import * as React from 'react'
import CourseProgramm from './CourseProgramm'
import PhotoState from '../HelpComponents/PhotoState';
import AddPhoto from './AddPhoto';
import PreviewClass from './PreviewClass';

class AddClass extends React.Component{

    constructor(props)
    {
        console.log("AddClass")
        super(props);
        this.state={
            photo:new PhotoState()
        }
    }

    render()
    {
        return(
            <Grid
            container        
            justifyContent="flex-start"
            direction="column"
            alignItems='center'
            rowGap={10}
            >
                <Grid item xs='auto'>
                    <Grid item>
                        <PreviewClass state={{photo:this.state.photo}}/>
                    </Grid>
                    <Grid item>


                    </Grid>
                </Grid>
                <Grid item style={{width:'100%'}}>
                    <CourseProgramm/>
                </Grid>

            </Grid>
        )
    }
}

export default AddClass