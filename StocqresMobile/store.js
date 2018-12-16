import { createStore, applyMiddleware, combineReducers } from 'redux';
import authenticationReducer from './ducks/authentication';
import axiosMiddleware from 'redux-axios-middleware';
import axios from 'axios';

const client = axios.create({
  baseURL: 'http://10.0.2.2:5000/api',
  //baseURL: 'https://localhost:443/api',
  responseType: 'json',
  headers:{
    // 'Content-Type': 'application/x-www-form-urlencoded',
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  }
})

const reducers = combineReducers({authentication:authenticationReducer});
const store = createStore(reducers, applyMiddleware(axiosMiddleware(client)));

export default store;