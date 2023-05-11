import axios from "axios";
import React from "react";
import { BaseUrl } from "../../App";




export default class ControllerGrid extends React.Component
{
    constructor(props)
    {
        super(props)
        console.log("controllerGrid");
        this.state={Request:null}
        this.state.Request= axios.create({
            baseURL:BaseUrl,
            headers:{ 'Content-Type': 'application/json' },
            responseType: 'stream'
        })
    }

    async GetControllerData()
    {
        let response = await fetch(BaseUrl+"/api/empty");
        let streamReader= response.body.getReader();

        while(true)
        {
            const {done, value} = await streamReader.read();

            if (done) {
              break;
            }
          
            console.log(value)
          }
    }

    render()
    {
        this.GetControllerData();
        return <div>

        </div>
    }
}