import axios from "axios";

export const types = {
  GET_STOCKS_LIST: "stocqres/login/GET_STOCKS_LIST",
  GET_STOCKS_LIST_SUCCESS: "stocqres/login/GET_STOCKS_LIST_SUCCESS",
  GET_STOCKS_LIST_FAIL: "stocqres/login/GET_STOCKS_LIST_FAIL",
  STOCK_LIST_RECEIVED: "stocqres/stocks/STOCK_LIST_RECEIVED"
};

const initialState = {
  loading: false,
  stockList: [],
};

export default function stocksReducer(
    state = { initialState },
    action
  ) {
    switch (action.type) {
      case types.GET_STOCKS_LIST:
        return { ...state, loading: true };
      case types.GET_STOCKS_LIST_SUCCESS:
        return {
          ...state,
          loading: false,
          success: true
        };
      case types.GET_STOCKS_LIST_FAIL:
        return { ...state, loading: false, success: false };
      case types.STOCK_LIST_RECEIVED:
        return { ...state, loading: false, success: false, stockList: action.stockList };
      default:
        return state;
    }
  }

  export function assignStockList(stockList){
      var parsedList = JSON.parse(stockList);
      return dispatch =>{
          dispatch(stockListReceived(parsedList.Items))
      }
  }

  const stockListReceived = (stockList) => ({
      type: types.STOCK_LIST_RECEIVED,
      stockList: stockList
  })
  