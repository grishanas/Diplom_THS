import LoadPhoto from "./LoadPhoto"


class LesonDescription{

    Lang
    Description
    ClassName
    constructor(props)
    {

    }

    setState(state)
    {

    }

    getState()
    {

    }
}


class Leson {

    #ShowTime
    #EndShowTime
    #PreviewPhoto
    #StartTime
    #EndTime
    #isUpdate
    #LesonDescription
    #id
    #IsLoad
    constructor(props)
    {
        this.#isUpdate=false;
        this.#PreviewPhoto=new LoadPhoto();

    }

    isUpdate()
    {
        return this.#isUpdate
    }

    setTime(state)
    {


    }

    setLesonDescription(newLeson)
    {
        if(this.#LesonDescription!=undefined)
        {
            let i=0;
            for(i;i<this.#LesonDescription.length;i++)
            {
                if(this.#LesonDescription[i].Lang === newLeson.Lang)
                {
                    this.#LesonDescription[i].setState(newLeson);
                    break
                }
            }
            if((i)=== this.#LesonDescription.length)
            {
                this.#LesonDescription.push(new LesonDescription(newLeson));
            }
        }
        else
        {
            this.#LesonDescription= new Array();
            this.#LesonDescription.push(new LesonDescription(newLeson));
        }
        this.#isUpdate=true;
    }

    setPhoto(photo)
    {

    }

    setState(state)
    {

    }

    getState()
    {
        return{  
                'ShowTime':this.#ShowTime,
                'EndShowTime':this.#EndShowTime,
                'PreviewPhoto':this.#PreviewPhoto,
                'StartTime':this.#StartTime,
                'EndTime':this.#EndTime,
                'LesonDescription':this.#LesonDescription
            }

    }

}


export {Leson,LesonDescription}