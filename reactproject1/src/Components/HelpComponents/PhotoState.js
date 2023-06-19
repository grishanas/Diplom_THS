import axios from "axios";

class PhotoState
{
    #isStateSav;
    #file
    #lang
    #anchor
    #url
    constructor(props)
    {
        this.#isStateSav=false;
    }

    isSaveState()
    {
        return this.#isStateSav;
    }

    setUrl(URL)
    {
        this.#url=URL;
    }

    setState(state)
    {
        this.#isStateSav=true;
        this.#file=state.file;
       
    }

    getState()
    {
        return {'file':this.#file,'anchor':this.#anchor,'lang':this.#lang};
    }

    

    async sendPhoto()
    {
        if(!this.#file) return null;
        if(!this.#url) return null;

            try
            {
                let formdata= new FormData();
                formdata.append('file',this.#file);
 
                let req= axios.create();
                let response = await req.post(this.#url,formdata);
            }
            catch(e)
            {
                console.error(e.message);
                return "Error";
            }
            return "OK";
    }


}

export default PhotoState;