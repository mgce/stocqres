import React, {Component} from 'react';
import {StyleSheet} from 'react-native';
import {default as LoginScreen} from './screens/LoginScreen';
import {default as MainScreen} from './screens/MainScreen';
import { Provider} from 'react-redux';
import {default as store} from './store'

type Props = {};
export default class App extends Component<Props> {
  render() {
    return (
      <Provider store={store}>
        <LoginScreen/>
      </Provider>
    );
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  instructions: {
    textAlign: 'center',
    color: '#333333',
    marginBottom: 5,
  },
});
