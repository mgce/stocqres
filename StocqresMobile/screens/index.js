import {Navigation} from 'react-native-navigation';
import { Provider} from 'react-redux';
import {default as store} from '../store'

export function registerScreens(){
  Navigation.registerComponentWithRedux('Login', () => require('./LoginScreen').default, Provider, store);
  Navigation.registerComponentWithRedux('Main', () => require('./MainScreen').default, Provider, store);
  Navigation.registerComponentWithRedux('Initializing', () => require('./Initializing').default, Provider, store);
  Navigation.registerComponentWithRedux('Register', () => require('./RegisterScreen').default, Provider, store);
}