import { createStore, applyMiddleware, compose } from "redux";
import authenticationReducer from "./ducks/authentication";
import stocksReducer from "./ducks/stocks";
import walletReducer from "./ducks/wallet";
import investorReducer from "./ducks/investor";
import axiosMiddleware from "redux-axios-middleware";
import axios from "axios";
import { persistCombineReducers } from "redux-persist";
import storage from "redux-persist/es/storage";
import thunk from "redux-thunk";

const client = axios.create({
  baseURL: "http://10.0.2.2:5000/api",
  responseType: "json",
  headers: {
    "Content-Type": "application/json",
    Accept: "application/json"
  },
});

const persistConfig = {
  key: "root",
  storage
};

const persistedReducer = persistCombineReducers(persistConfig, {
  authentication: authenticationReducer,
  stocks: stocksReducer,
  wallet: walletReducer,
  investor:investorReducer
});

const composeEnhancer = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const enhancer = composeEnhancer(
  applyMiddleware(axiosMiddleware(client)),
  applyMiddleware(thunk)
);

const store = createStore(persistedReducer, enhancer);

export default store;
