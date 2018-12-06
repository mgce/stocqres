import React, {Component} from 'react';
import {default as LoginScreen} from './LoginScreen';
import { Provider} from 'react-redux';
import {default as store} from '../store'

export default class Auth extends Component {
  render() {
    return (
      <Provider store={store}>
        <LoginScreen/>
      </Provider>
    );
  }
}
