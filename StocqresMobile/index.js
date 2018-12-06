import {Navigation} from 'react-native-navigation';
import {registerScreens} from './screens/index';

registerScreens();

Navigation.events().registerAppLaunchedListener(() => {
  Navigation.setRoot({
    root: {
      stack:{
          options:{
            topBar:{
                visible: false
            }
        },
        children:[
          {
            component: {
            name: 'App'
          }
        }
        ]
      }
    },
  });
}); 