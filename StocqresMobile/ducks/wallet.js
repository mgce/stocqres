import httpClient from "./httpClient";
import storage from "redux-persist/es/storage";
import { persistReducer } from 'redux-persist';

export const types = {
    GET_WALLET_DETAILS: "stocqres/wallet/GET_WALLET_DETAILS",
    GET_WALLET_DETAILS_SUCCESS: "stocqres/wallet/GET_WALLET_DETAILS_SUCCESS",
    GET_WALLET_DETAILS_FAIL: "stocqres/wallet/GET_WALLET_DETAILS_FAIL",
  };

const initialState = {
    myStocks: []
  };

  function walletReducer(state = { initialState }, action) {
    switch (action.type) {
      case types.GET_WALLET_DETAILS:
        return { ...state, loading: true };
      case types.GET_WALLET_DETAILS_SUCCESS:
        return {
          ...state,
          loading: false,
          success: true,
          myStocks: action.myStocks
        };
      case types.GET_WALLET_DETAILS_FAIL:
        return { ...state, loading: false, success: false };
      
        default:
        return state;
    }
  }
  
  export default walletReducer;
  

  
export function getWalletDetails(id){
    return (dispatch) => {
      dispatch(request());
      httpClient.get(`investors/${id}/wallet`).then(res =>{
        dispatch(success(res.data))
      }, error => {
        dispatch(failure(error))
      })
    }
  
    function request(){ return { type: types.GET_MY_STOCKS}}
    function success(stockDetails) { return { type: types.GET_MY_STOCKS_SUCCESS, stockDetails } }
    function failure(error) { return { type: types.GET_MY_STOCKS_FAIL, error } }
  }