import {Navigation} from 'react-native-navigation';
import {registerScreens} from './screens/index';

registerScreens();

Navigation.events().registerAppLaunchedListener(() => {
  Navigation.setRoot({
    root: {
      component: {
        name: 'App'
      }
    },
  });
}); 