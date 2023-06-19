
class CourseProgramm{

    lang
    #Lesons
    constructor(props)
    {
        this.lang=props
    }
    


    deletePoint(Point)
    {
        console.log(Point);
        this.#Lesons[Point.lesonid].points.splice(Point.pointid,1);
        return this.#Lesons;
    }

    deleteLeson(leson)
    {
        this.#Lesons.splice(leson.lesonid);
        return this.#Lesons;

    }

    createLeson()
    {
        return  {'header':undefined,'points':new Array()};
    }

    addCourseProgramm(Leson)
    {
        if(this.#Lesons!=undefined)
        {
            this.#Lesons.push(this.createLeson());
        }
        else
        {
            this.#Lesons= new Array();
            this.#Lesons.push(this.createLeson());
        }
        console.log(this.#Lesons);


    }

    setCourseProgrammPoint(leson)
    {
        if(this.#Lesons[leson.lesonid].points!=undefined)
        {
            let i=0;
            if(leson.pointid!=undefined)
            {
                this.#Lesons[leson.lesonid].points[leson.pointid]={'point':leson.point}
            }
            else
            {
                this.#Lesons[leson.lesonid].points.push({'point':null})
            }
        }
        else
        {
            this.#Lesons[leson.lesonid].points= new Array();
            this.#Lesons[leson.lesonid].points.push({point:leson.point})
        }
        return this.#Lesons;
    }

    getState()
    {
        
    }

    setCourseProgrammHeader(leson)
    {
        this.#Lesons[leson.id].header=leson.header;
    }

    getLesons()
    {
        return this.#Lesons;
    }

    getLang()
    {
        return this.lang;
    }


}


class CourseProgramms
{
    #isEmpty
    #courseProgramm
    #header
    constructor(props)
    {
        this.#isEmpty=true;
    }

    setLang(lang)
    {
        this.#isEmpty=false;
        if(this.#courseProgramm!=undefined)
        {
            let i=0;
            for(i;i<this.#courseProgramm.length;i++)
            {
                if(this.#courseProgramm[i].lang===lang)
                {
                    return this.#courseProgramm[i];
                }

            }
            if(i==this.#courseProgramm.length)
            {
                let tmp= new CourseProgramm(lang);
                this.#courseProgramm.push(tmp);
                return tmp;
            }
        }
        else
        {
            let tmp= new CourseProgramm(lang);
            this.#courseProgramm= new Array();
            this.#courseProgramm.push(tmp);
            return tmp;
        }


    }

    setHeader(header)
    {
        this.#header=header;
    }

    changeLeson(LesonDescription)
    {

    }

    isEmpty()
    {
        return this.#isEmpty;
    }

}

export {CourseProgramms,CourseProgramm}
