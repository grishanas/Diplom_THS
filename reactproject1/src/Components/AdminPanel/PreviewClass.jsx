import { Button, FormControl, Grid, InputBase, InputLabel, MenuItem, NativeSelect, OutlinedInput, Paper, Select, TextField, Typography } from '@mui/material';
import * as React from "react"  
import { Leson } from "../HelpComponents/Class";
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import AddPhoto from './AddPhoto';


class PreviewClass extends React.Component{

    constructor(props)
    {
  
        super(props);
        let tmp=new Leson();
        //this.state.leson=props.Leson.getState();
        this.state={
            leson:tmp.getState(),
        };
        this.state.clearedDate=null;

    }


    render()
    {
    
        return(
           <Paper style={{width:700}}>
               <Grid container columnSpacing={2} direction="row"

               >
                    <Grid item xs='auto'>
                        <AddPhoto state={this.state.leson.PreviewPhoto}/>
                    </Grid>
                    <Grid item xs={12} sm container>
                        <Grid item xs   container
                        direction="column"
                        justifyContent="flex-start"
                        rowSpacing={4}
                        >
                            <Grid item>
                                <FormControl fullWidth>
                                    <InputLabel>Choose lang</InputLabel>
                                    <Select
                                        label="Choose lang"
                                    >    
                                        <MenuItem key={10} value={10}>Ten</MenuItem>
                                        <MenuItem key={30} value={20}>Twenty</MenuItem>
                                        <MenuItem key={20} value={30}>Thirty</MenuItem>
                                    </Select>
                                </FormControl>
                            </Grid>
                            <Grid item>
                            <Grid item xs container direction="row">
                                <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <DatePicker
                                    renderInput={(props) => <TextField {...props} />}
                                    label="DateTimePicker"
                                    value={this.state.clearedDate}
                                    onChange={(newValue) => {
                                    this.setState({clearedDate:newValue})
                                    }}/>
                                </LocalizationProvider>
                                </Grid>
                                <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <DatePicker
                                    renderInput={(props) => <TextField {...props} />}
                                    label="DateTimePicker"
                                    value={this.state.clearedDate}
                                    onChange={(newValue) => {
                                    this.setState({clearedDate:newValue})
                                    }}/>
                                </LocalizationProvider>
                                </Grid>
                                <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <DatePicker
                                    renderInput={(props) => <TextField {...props} />}
                                    label="DateTimePicker"
                                    value={this.state.clearedDate}
                                    onChange={(newValue) => {
                                    this.setState({clearedDate:newValue})
                                    }}/>
                                </LocalizationProvider>
                                </Grid>
                                <Grid item xs={6}>
                                <LocalizationProvider dateAdapter={AdapterDateFns}>
                                <DatePicker
                                    renderInput={(props) => <TextField {...props} />}
                                    label="DateTimePicker"
                                    value={this.state.clearedDate}
                                    onChange={(newValue) => {
                                    this.setState({clearedDate:newValue})
                                    }}/>
                                </LocalizationProvider>
                                </Grid>
 

                            </Grid>
                            </Grid>
                            <Grid item alignItems="center">
                                <InputBase placeholder='Название' value={this.state.ClassName} onChange={(e)=>{this.changeLabel('personalData',e)}}/>
                                <InputBase multiline maxRows={4} placeholder='Описание' value={this.state.Description} onChange={(e)=>{this.changeLabel('description',e)}}/>
                            </Grid>
                            <Grid item xs container alignItems="flex-end">
                                <Button>
                                    Press
                                </Button>
                                <Button>
                                    second Press
                                </Button>
                            </Grid>
                        </Grid>
                    </Grid>

                </Grid>
           </Paper>
        )

    }

}

export default PreviewClass