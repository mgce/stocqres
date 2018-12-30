import httpClient from "./httpClient";
import storage from "redux-persist/es/storage";
import { persistReducer } from 'redux-persist';

export const types = {
    GET_WALLET_DETAILS: "stocqres/wallet/GET_WALLET_DETAILS",
    GET_WALLET_DETAILS_SUCCESS: "stocqres/wallet/GET_WALLET_DETAILS_SUCCESS",
    GET_WALLET_DETAILS_FAIL: "stocqres/wallet/GET_WALLET_DETAILS_FAIL",
    CREATE_WALLET: "stocqres/wallet/CREATE_WALLET",
    CREATE_WALLET_SUCCESS: "stocqres/wallet/CREATE_WALLET_SUCCESS",
    CREATE_WALLET_FAIL: "stocqres/wallet/CREATE_WALLET_FAIL",
  };

const initialState = {
    wallet: {}
  };

  function walletReducer(state = initialState, action) {
    switch (action.type) {
      case types.GET_WALLET_DETAILS:
        return { ...state, loading: true };
      case types.GET_WALLET_DETAILS_SUCCESS:
        return {
          ...state,
          loading: false,
          success: true,
          wallet: action.wallet
        };
      case types.GET_WALLET_DETAILS_FAIL:
        return { ...state, loading: false, success: false };
      
      case types.CREATE_WALLET:
        return { ...state, loading: true };
      case types.CREATE_WALLET_SUCCESS:
        return {
          ...state,
          loading: false,
          success: true
        };
      case types.CREATE_WALLET_FAIL:
        return { ...state, loading: false, success: false };
      
        default:
        return state;
    }
  }
  
  export default walletReducer;
  

  
export function getWalletDetails(){
    return (dispatch, getState) => {
      dispatch(request());
      httpClient.get(`/investors/wallet`).then(res =>{
        dispatch(success(res.data))
      }, error => {
        dispatch(failure(error))
      })
    }
  
    function request(){ return { type: types.GET_WALLET_DETAILS}}
    function success(wallet) { return { type: types.GET_WALLET_DETAILS_SUCCESS, wallet } }
    function failure(error) { return { type: types.GET_WALLET_DETAILS_FAIL, error } }
  }

  export function createWallet(amount){
    return (dispatch) => {
      const data = {amount:amount}
      dispatch(request());
      httpClient.post(`/investors/wallet`, data).then(res =>{
        dispatch(success(res.data))
        getWalletDetails(investorId);
      }, error => {
        dispatch(failure(error))
      })
    }
  
    function request(){ return { type: types.CREATE_WALLET}}
    function success() { return { type: types.CREATE_WALLET_SUCCESS } }
    function failure(error) { return { type: types.CREATE_WALLET_FAIL, error } }
  }