import { createStore, applyMiddleware } from "redux";
import authenticationReducer from "./ducks/authentication";
import axiosMiddleware from "redux-axios-middleware";
import axios from "axios";
import {persistCombineReducers} from "redux-persist";
import storage from 'redux-persist/es/storage';

const client = axios.create({
  baseURL: "http://10.0.2.2:5000/api",
  responseType: "json",
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  }
});

const persistConfig = {
  key: "root",
  storage
};

const persistedReducer = persistCombineReducers(persistConfig, {
  authentication: authenticationReducer
});

const store = createStore(
  persistedReducer,
  applyMiddleware(axiosMiddleware(client))
);

export default store;