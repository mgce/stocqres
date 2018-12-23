import httpClient from "./httpClient";
import storage from "redux-persist/es/storage";
import { persistReducer } from 'redux-persist';

export const types = {
  GET_STOCKS_DETAILS: "stocqres/stocks/GET_STOCKS_DETAILS",
  GET_STOCKS_DETAILS_SUCCESS: "stocqres/stocks/GET_STOCKS_DETAILS_SUCCESS",
  GET_STOCKS_DETAILS_FAIL: "stocqres/stocks/GET_STOCKS_DETAILS_FAIL",
  STOCK_LIST_RECEIVED: "stocqres/stocks/STOCK_LIST_RECEIVED",
  BUY_STOCKS: "stocqres/stocks/BUY_STOCKS",
  BUY_STOCKS_SUCCESS: "stocqres/stocks/BUY_STOCKS_SUCCESS",
  BUY_STOCKS_FAIL: "stocqres/stocks/BUY_STOCKS_FAIL",
};

const initialState = {
  loading: false,
  stockList: [],
  stockDetails: {
    stockQuantity: null
  }
};

const persistConfig = {
  key: 'stocks',
  storage: storage,
  blackList: ['stockDetails']
}

function stocksReducer(state = { initialState }, action) {
  switch (action.type) {
    case types.GET_STOCKS_DETAILS:
      return { ...state, loading: true };
    case types.GET_STOCKS_DETAILS_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        stockDetails: action.stockDetails
      };
    case types.GET_STOCKS_DETAILS_FAIL:
      return { ...state, loading: false, success: false };
    case types.STOCK_LIST_RECEIVED:
      return {
        ...state,
        loading: false,
        success: false,
        stockList: action.stockList
      };
    default:
      return state;
  }
}

export default persistReducer(persistConfig, stocksReducer);

export function getStockDetails(code){
  return (dispatch) => {
    dispatch(request());
    httpClient.get('/company/' + code).then(res =>{
      dispatch(success(res.data))
    }, error => {
      dispatch(failure(error))
    })
  }

  function request(){ return { type: types.GET_STOCKS_DETAILS}}
  function success(stockDetails) { return { type: types.GET_STOCKS_DETAILS_SUCCESS, stockDetails } }
  function failure(error) { return { type: types.GET_STOCKS_DETAILS_FAIL, error } }
}

export function buyStocks(data){
  return{
    type:types.BUY_STOCKS,
    payload:{
      request:{
        url:'/orders/buy',
        method: 'POST',
        data:{
          WalletId: "",
          CompanyId: data.companyId,
          Quantity: data.quantity
        }
      }
    }
  }
}

export function assignStockList(stockList) {
  var parsedList = JSON.parse(stockList);
  return (dispatch, getState) => {
    const {stocks} = getState();
    var comparedStockList = compareStockPrices(parsedList.Items, stocks.stockList)
    dispatch(stockListReceived(comparedStockList));
  };
}

const stockListReceived = stockList => ({
  type: types.STOCK_LIST_RECEIVED,
  stockList: stockList
});

compareStockPrices = (newStockList, oldStockList) => {
    if(oldStockList === undefined)
        return newStockList;
        
    for (let index = 0; index < newStockList.length; index++) {
        const oldElement = oldStockList[index];
        const newElement = newStockList[index];

        newElement.Id = index;
        newElement.PercentageRate = parseFloat((oldElement.Price / newElement.Price).toFixed(3));
        newElement.Price = Math.round(newElement.Price * 100) / 100

        if(oldElement.Price > newElement.Price)
            newElement.Increased = false;
        else if(oldElement.Price < newElement.Price)
            newElement.Increased = true;
        else
            newElement.Increased = oldElement.Increased;


    }
    return newStockList;
}
