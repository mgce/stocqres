import { createStore, applyMiddleware, combineReducers } from 'redux';
import loginReducer from './ducks/login';
import axiosMiddleware from 'redux-axios-middleware';
import axios from 'axios';

const client = axios.create({
  baseUrl: 'https://localhost:44327/api',
  responseType: 'json'
})

const reducers = combineReducers({login:loginReducer});
const store = createStore(reducers, applyMiddleware(axiosMiddleware(client)));

export default store;