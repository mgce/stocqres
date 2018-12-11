import { createStore, applyMiddleware, combineReducers } from 'redux';
import loginReducer from './ducks/login';
import axiosMiddleware from 'redux-axios-middleware';
import axios from 'axios';

const client = axios.create({
  baseURL: 'https://192.168.0.1:44327/api',
  //baseURL: 'https://localhost:443/api',
  responseType: 'json',
  headers:{
    // 'Content-Type': 'application/x-www-form-urlencoded',
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
})

const reducers = combineReducers({login:loginReducer});
const store = createStore(reducers, applyMiddleware(axiosMiddleware(client)));

export default store;