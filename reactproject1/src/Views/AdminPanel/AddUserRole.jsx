import axios from "axios";
import React, { createRef } from "react";
import Tippy from "@tippyjs/react";
import { BaseUrl } from "../../App";


export default class AddUserRole extends React.Component
{
    constructor(props)
    {

        super(props);
        this._text="Добавить роль";
        this.state={Request:null};
        this.state={visible:false,hide:true,AvalibelRoles:null}
        if(this.props.value!==undefined)
            this.state={hide:false};   
        this.state.Request = axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
        });
        

        
    }

    get text()
    {
        return this._text
    }

    async addRole(role)
    {
        console.log(this.state);
        this.state.Request.post("/api/User/AddRoleToUser",{
            user:this.props.id,
            userRole:role.id
        }).then((e)=>{console.log(e)});    
    }

    AddRoleContent()
    {
        let tmp = this.props.GetUser(this.props.id);
        let avalibleRoles= this.props.GetRoles();
        let count=0;
        return <div>
            {avalibleRoles? 
                avalibleRoles.map((element)=>{
                    let i=0;
                    count++;
                    for(;i<tmp.userRoles.length;i++)
                    {
                        if(element.id == tmp.userRoles[i].id)
                        {
                            return null;
                        }
                    }
                    return <button onClick={(e)=>this.addRole(element)}>{element.description}</button>
                })
            :
            <div>
                {count>0?<p> нет доступных ролей</p>:<p>Загрузка ролей</p>}
            </div>}
        </div>
    }

    render()
    {

        return <div>
            <Tippy
                content={this.AddRoleContent()}
                visible = {this.state.visible}
                onClickOutside={(e)=>this.setState({visible:false})}
                allowHTML={true}
                arrow={false}
                appendTo={document.body}
                interactive={true}
                placement="right"
            >
                <button onClick={(e)=>{this.state.visible ? this.setState({visible:false}):this.setState({visible:true})}}>
                    {this._text}
                </button>    
            </Tippy>
        </div>

    }
}