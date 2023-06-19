import {TeacherLanguage} from "./LanguageBlock"

class addTeacher
{

    #Photo
    #Languages
    #isTeacherSafe
    constructor(props)
    {
        this.#isTeacherSafe=false;
    }

    setState(state)
    {
        this.#isTeacherSafe= true;
        if(this.#Languages!=undefined)
        {
        
            let i=0;
            for(i;i<this.#Languages.length;i++)
            {
                if(this.#Languages[i].lang==state.lang)
                {
                    this.#Languages[i].setState(state);
                    break;
                }
            }
            if((i)==this.#Languages.length)
            {
                this.#Languages.push(new TeacherLanguage(state))
            }
        }else
        {
            this.#Languages= new Array();
            this.#Languages.push(new TeacherLanguage(state))
        }
        this.#Photo=state.photo;
    }

    getTeacherInformation(language)
    {

        if(this.#Languages==undefined)
        {
            return new TeacherLanguage({lang:language});
        }
        let i=0;
        for(i;i<this.#Languages.length;i++)
        {

            if(this.#Languages[i].lang==language)
            {
                return this.#Languages[i];
            }
        }
        return new TeacherLanguage({lang:language});
    }

    getState()
    {
        return {
            'Languages':this.#Languages,
            'photo':this.#Photo,
        }
    }

}

export default addTeacher