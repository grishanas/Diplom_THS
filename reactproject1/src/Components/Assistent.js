import React from 'react'
import { Paper } from '@mui/material';

export class Assistent extends React.Component{

    constructor(props){
        super(props)
        this.state = {IsOpen:false}; 

        this.ChangeState = this.ChangeState.bind(this);
    }

    ChangeState()
    {
        this.setState(currentState=>({
            IsOpen:!currentState.IsOpen
        }));
    }

    render()
    {
    
    return (
        <div className='Assistent'>

        <button onClick={this.ChangeState}>{this.state.IsOpen?<img src='https://image.shutterstock.com/image-vector/message-icon-mobile-phone-chat-260nw-1714039993.jpg'/>:<img src='https://image.shutterstock.com/image-vector/message-icon-mobile-phone-chat-260nw-1714039993.jpg'/>}</button>
            {this.state.IsOpen ?
                
                <Paper className='Assistent-popUpWindow'>
                    <div className='Assistent-popUpWindow-descript'>
                    <h1>
                        {this.props.title ? this.props.title:'Title'}
                    </h1>
                    <p>
                        {this.props.description ? this.props.description:'dsa'}
                    </p>
                    </div>
                    <div className='Assistent-popUpWindow-ref'>
                        {this.props.ref ? this.props.ref.map((value)=>(
                            <div>
                            <img src={value.ref}/>                           
                            <p>
                                {value.title}
                            </p>
                            </div>
                        )):<p>Loading socia media</p>}

                    </div>
                </Paper>
            : <p></p>}
        </div>
    );   
}
 
}