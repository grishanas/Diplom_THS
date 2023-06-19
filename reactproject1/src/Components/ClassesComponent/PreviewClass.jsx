import { CardHeader,Card, Typography, Avatar, CardContent, CardActionArea, Button } from '@mui/material'
import React from 'react'
import { Leson } from '../HelpComponents/Class';


class PreviewClass extends React.Component{


    /**
     * requed previewPhoto, allowedLanguage,className,startTime,moreInformation
     * @param {*} props 
     */
    constructor(props)
    {
        super(props);
        this.state= new Leson();
        
    }

    render()
    {
        return (
            <Card style={{maxWidth:400}}>
                <CardHeader
                    avatar={
                        <Avatar
                            //src="https://static.tildacdn.com/tild6461-3563-4734-a165-633664336138/Asset_1.svg"
                            variant='rounded'
                        >
                        </Avatar>
                    }

                />
                <CardContent>
                    <Typography>
                        some information
                    </Typography>

                </CardContent>
                <CardActionArea>
                    <Button>
                        more information
                    </Button>

                </CardActionArea>
                
                

            </Card>
        )

    }
}

export default PreviewClass