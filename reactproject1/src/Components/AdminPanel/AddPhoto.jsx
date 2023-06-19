import { Box } from '@mui/system';
import * as React from 'react'
import AddAPhotoIcon from '@mui/icons-material/AddAPhoto';
import { Button, IconButton, Paper, Typography } from '@mui/material';
import axios from 'axios'
import CloseIcon from '@mui/icons-material/Close';

class AddPhoto extends React.Component{


    constructor(props)
    {
        console.log(props);
        super(props);
        this.state={
            file:null,
            FileAsUrl:null,
            isVisibleCancel:false,

        }
        if(this.props.state.isSaveState!=undefined)
        {
            if(this.props.state.isSaveState())
            {
            this.state=this.props.state.getState();
            let reader= new FileReader();
            reader.onloadend=()=>{
                let res=reader.result;
                this.setState({FileAsUrl:res});  
            }
            reader.readAsDataURL(this.state.file);
            }
        }
        else
            if(props.state.isLoad!=undefined)
            {
                if(props.state.isLoad())
                {
                    this.state={
                        FileAsUrl:this.props.state.getUrl()
                    }
                }
            }
            else
                {
                
            }

    }



    CleanState()
    {
        this.setState({file:null});
        this.setState({FileAsUrl:null});
    }

    componentWillUnmount()
    {
        //this.props.state.setState(this.state);
    }

    Prerander(event){


        
        let reader= new FileReader();
        reader.onloadend=()=>{
            let res=reader.result;
            this.setState({FileAsUrl:res});  
        }
        reader.readAsDataURL(event.target.files[0]);
        this.setState({file:event.target.files[0]});
        this.props.state.setState(this.state);
    }




    render()
    {
       return(
       <Box
        style={{
            display:'flex',
            alignItems:'center',
            justifyContent:'center',
        }}

       >
        {(this.state.FileAsUrl!=null)?
        /* вывод окна для добавления фото */
        <Box>
            <Paper 
            style={{
                backgroundImage:`url(${this.state.FileAsUrl})`,
                height:400,
                width:300,
                backgroundRepeat:'no-repeat',
                backgroundSize:'cover',
            }}
            onMouseOver={(e)=>this.setState({isVisibleCancel:true})}
            onMouseOut={(e)=>this.setState({isVisibleCancel:false})}>
            {(this.state.isVisibleCancel)?
            <IconButton onClick={(e)=>this.CleanState()}>
                <CloseIcon/>
            </IconButton>
            : null
            }
            </Paper>
            {(this.state.file!=null)?<Typography>{this.state.file.name}</Typography>:null}
            

        </Box>:
        /* вывод самого фото */
        
        <Paper
            style={{
                height:400,
                width:300,
                display:'flex',
                alignItems:'center',
                justifyContent:'center',
            }}
        >
           
            <input
            accept="image/*" 
            style={{ display: 'none' }}
            id="raised-button-file"
            type="file"
            onChange={(e)=>this.Prerander(e)}
            />
                <label htmlFor="raised-button-file">
                    <IconButton variant="raised" component="span">
                        <AddAPhotoIcon fontSize="small"/>
                    </IconButton>
                </label>     
        </Paper>}

       </Box>
       )
    }
}

export default AddPhoto;