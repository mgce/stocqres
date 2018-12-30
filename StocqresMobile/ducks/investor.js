import httpClient from "./httpClient";

export const types = {
  GET_INVESTOR_DETAILS: "stocqres/stocks/GET_INVESTOR_DETAILS",
  GET_INVESTOR_DETAILS_SUCCESS: "stocqres/stocks/GET_INVESTOR_DETAILS_SUCCESS",
  GET_INVESTOR_DETAILS_FAIL: "stocqres/stocks/GET_INVESTOR_DETAILS_FAIL",
};

const initialState = {
  loading: false,
  firstName:"",
  lastName:""
};

export default function stocksReducer(state = initialState , action) {
  switch (action.type) {
    case types.GET_INVESTOR_DETAILS:
      return { ...state, loading: true };
    case types.GET_INVESTOR_DETAILS_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        firstName: action.details.firstName,
        lastName: action.details.lastName,
      };
    case types.GET_INVESTOR_DETAILS_FAIL:
      return { ...state, loading: false, success: false };
    default:
      return state;
  }
}


export function getInvestorDetails(code){
  return (dispatch) => {
    dispatch(request());
    httpClient.get('/investors').then(res =>{
      dispatch(success(res.data))
    }, error => {
      dispatch(failure(error))
    })
  }

  function request(){ return { type: types.GET_INVESTOR_DETAILS}}
  function success(details) { return { type: types.GET_INVESTOR_DETAILS_SUCCESS, details } }
  function failure(error) { return { type: types.GET_INVESTOR_DETAILS_FAIL, error } }
}
