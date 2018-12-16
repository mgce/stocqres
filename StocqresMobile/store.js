import { createStore, applyMiddleware, combineReducers } from "redux";
import authenticationReducer from "./ducks/authentication";
import axiosMiddleware from "redux-axios-middleware";
import axios from "axios";
import { persistStore, persistReducer, persistCombineReducers, AsyncStorage } from "redux-persist";
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

const combinedReducers = combineReducers({
  authentication: authenticationReducer
});

const persistedReducer = persistCombineReducers(persistConfig, {
  authentication: authenticationReducer
});

const store = createStore(
  persistedReducer,
  applyMiddleware(axiosMiddleware(client))
);

export default store;

// export default() => {
//   let store = createStore(persistedReducer, applyMiddleware(axiosMiddleware(client)));
//   let persistor = persistStore(store);
//   return{store, persistor}
// }
