import { Toolbar,Box,Stack,CssBaseline,AppBar,Typography } from "@mui/material";
import React from "react";
import { Link } from "react-router-dom";


const Headers= [{url:"/AddController",value:"A"},{url:"B",value:"B"},{url:"C",value:"C"}];

export default class Header extends React.Component
{

    constructor(props)
    {
        super(props)
        this.state={navigationMenu:Headers};
    }

    render()
    {

        return (
            
        <Box sx={{ display: 'flex' }}>
            <CssBaseline/>
            <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
                <Toolbar>
                    <Box sx={{ display: { xs: 'none', sm: 'block' }}}>
                        <Link to="/" style={{ color:'#ff22f2',
                            textDecoration:'none',
                        }}>
                        <Typography >
                            Главная
                         </Typography>
                        </Link>  
                    </Box>
                    <Stack margin='auto' direction='row' spacing={2} >
                        {(this.state.navigationMenu)?
                        this.state.navigationMenu.map((item,index)=>(
                            
                        <Link
                            to={item.url}
                            className="nav-bar-list"
                        >
                            {item.value}
                       </Link>
                    )):null}
                    </Stack>
                </Toolbar>
            </AppBar>
        </Box>

            
        )
    }
}