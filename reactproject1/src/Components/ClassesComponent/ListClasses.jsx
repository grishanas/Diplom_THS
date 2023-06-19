import { Grid, Typography,Box, Stack, Button } from '@mui/material'
import React from 'react'
import PreviewClass from './PreviewClass';


class ListClasses extends React.Component{

    constructor(props)
    {
        super(props)
        this.state = { width: 0, height: 0 };
        this.updateWindowDimensions = this.updateWindowDimensions.bind(this);
    }

    componentDidMount() {
        this.updateWindowDimensions();
        window.addEventListener('resize', this.updateWindowDimensions);
    }
      
    componentWillUnmount() {
        window.removeEventListener('resize', this.updateWindowDimensions);
    }
      
      updateWindowDimensions() {
        this.setState({ width: window.innerWidth, height: window.innerHeight });
    }

    render()
    {
        return (
        <Grid item container style={{width:1000}} direction='column' justifyContent="center" margin='auto' spacing={4}>
            <Grid item container direction='column' justifyContent="center" marginBottom={10}>
                <Grid item>
                <Typography>
                    some Coursesdsadsaaaa dsadsadsa aaaaaaaaaaaaaaaaaa
                </Typography>
                </Grid>
                <Grid item>
                <Typography>
                    some about us
                </Typography>
                </Grid>
            </Grid>
            <Grid item>
                <Stack direction='row'>
                    <Button>
                        1
                    </Button>
                    <Button>
                        2
                    </Button>
                </Stack>
            </Grid>
            <Grid item>
                <PreviewClass/>


            </Grid>

        </Grid>);

    }
}

export default ListClasses