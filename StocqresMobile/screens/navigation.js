import { Navigation } from 'react-native-navigation'

export const goToAuth = () => Navigation.setStackRoot({
    root:{
        stack:{
            id: 'LoginScreen'
        }
    }
})

export const goToHome = () => Navigation.setStackRoot({
    root:{
        stack:{
            id: 'MainScreen'
        }
    }
})