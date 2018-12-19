import axios from "axios";

export const types = {
  GET_STOCKS_DETAILS: "stocqres/login/GET_STOCKS_DETAILS",
  GET_STOCKS_DETAILS_SUCCESS: "stocqres/login/GET_STOCKS_DETAILS_SUCCESS",
  GET_STOCKS_DETAILS_FAIL: "stocqres/login/GET_STOCKS_DETAILS_FAIL",
  STOCK_LIST_RECEIVED: "stocqres/stocks/STOCK_LIST_RECEIVED"
};

const initialState = {
  loading: false,
  stockList: [],
  stockDetails: {}
};

export default function stocksReducer(state = { initialState }, action) {
  switch (action.type) {
    case types.GET_STOCKS_DETAILS:
      return { ...state, loading: true };
    case types.GET_STOCKS_DETAILS_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        stockDetails: action.payload.data
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

export function getStockDetails(code){
    return{
        type: types.GET_STOCKS_DETAILS,
        payload:{
            request:{
                url: '/company/' + code,
                method: 'GET'
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
