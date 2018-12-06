import { Navigation } from 'react-native-navigation'

export const goToAuth = () => Navigation.setRoot({
    root:{
        stack:{
            id:'Authorization',
            options:{
                topBar:{
                    visible: false
                },
            },
            children:[
                {
                    component:{
                        name: 'Auth'
                    }
                }
            ]
            
        } 
    }
})

export const goToHome = () => Navigation.setStackRoot({
    root:{
        stack:{
            id: 'Main'
        }
    }
})