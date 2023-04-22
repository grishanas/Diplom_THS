import React from "react";
import Tippy from "@tippyjs/react";
import "./table.css"
import { Button, Popover, Typography } from "@mui/material";


export default class PopUpWindow extends React.Component{


    constructor(props)
    {
        super(props)
        this.state={hide:false,setAnchorEl:null,visible:false}

        
    }

    PopUpWindowContent()
    {

        return <div>

        </div>

    }

    Open(e)
    {
        this.setState({setAnchorEl:e.currentTarget})
    }

    Close()
    {
        this.setState({setAnchorEl:null})
    }

    render()
    {

        return <div hidden={this.state.hide} >
        <Button aria-describedby={'popover'} variant="text" onClick={(e)=>this.Open(e)}>
            Действие
        </Button>
        <Popover
            id={'popover'}
            open = {Boolean(this.state.setAnchorEl)}
            onClose={(e)=>this.Close()} 
            anchorEl={this.state.setAnchorEl}
            anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
            }}
        >
            {this.props.PopUp.map((item)=> {return item(this.props?this.props.data.id:undefined,
                this.props.context?this.props.context.GetRoles:undefined,
                this.props.context?this.props.context.GetUser:undefined)} )}
        </Popover>    
        </div>

    }
}