
class TeacherLanguage
{
    constructor(props)
    {
        this.lang=props.lang;
        this.personalData=props.personalData?props.personalData:'';
        this.teacherDescripion=props.teacherDescripion?props.teacherDescripion:'';
    }

    setState(state)
    {
        this.personalData=state.personalData?state.personalData:'';
        this.description = state.teacherDescripion?state.teacherDescripion:'';
    }

    getState()
    {
        return {
            'lang':this.lang,
            'personalData':this.personalData,
            'teacherDescripion':this.teacherDescripion,
        }
    }
}


export {TeacherLanguage}