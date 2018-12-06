import {Navigation} from 'react-native-navigation';

export function registerScreens(){
  Navigation.registerComponent('Login', () => require('./LoginScreen').default);
  Navigation.registerComponent('Main', () => require('./MainScreen').default);
  Navigation.registerComponent('Initializing', () => require('./Initializing').default);
  Navigation.registerComponent('App', () => require('../App').default);
  Navigation.registerComponent('Auth', () => require('./Auth').default);
}