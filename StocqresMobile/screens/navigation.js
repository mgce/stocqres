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
                        name: 'Login'
                    }
                }
            ]
            
        } 
    }
})

export const goToHome = () => Navigation.setRoot({
    root:{
        stack:{
            id: 'Main',
            options:{
                topBar:{
                    visible: true,
                    title: 'Stock Exchange'
                }
            },
            children:[
                {
                    component:{
                        name:'Main'

                    }
                }
            ]
        }
    }
})