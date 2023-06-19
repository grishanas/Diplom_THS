import React,{useState} from "react";
import { MenuItem, Select,NativeSelect } from "@mui/material";


const Navigatio=(props)=>{

    const [Language, ChangeLanguage] = React.useState(String);

    return(
        <nav id="menu">
            <div className="container">
                <div className="navigationTabBar">
                    <div className="NavigationTitle">
                        {console.log(props)}
                        <h1>{props.Navigation ? props.Navigation.NavTitle : 'Title Load'}</h1>
                    </div>
                   <ul className="nav-bar-list">
                       {props.Navigation ? props.Navigation.ref.map((value)=>(
                        <li>
                        <a href={value.link} className="NavBarComponent">
                            {value.text}
                        </a>

                       </li>
                       )):<li></li>}

                   </ul>

                    <div className="nav-bar-right-part">
                   <NativeSelect 
                   id="Nav-bar-language-selector"
                   defaultValue={props.Language[0]}
                            onChange={(e)=>{ChangeLanguage(e.target.value)}}>
                            <option value={props.Language[0]} >{props.Language[0]}</option>
                            <option value={props.Language[1]} >{props.Language[1]}</option>
                    </NativeSelect> 
                    </div>
                        
                </div>
            </div>

        </nav>
    )
}

export default Navigatio