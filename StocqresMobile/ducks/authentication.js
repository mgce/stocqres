import httpClient from "./httpClient";
import { AsyncStorage } from "react-native";
import {goToHome} from '../screens/navigation'
import constants from "../common/constants";
import jwt_decode from "jwt-decode";

export const types = {
  LOGIN: "stocqres/login/LOGIN",
  LOGIN_SUCCESS: "stocqres/login/LOGIN_SUCCESS",
  LOGIN_FAIL: "stocqres/login/LOGIN_FAIL",
  REGISTER: "stocqres/login/REGISTER",
  REGISTER_SUCCESS: "stocqres/login/REGISTER_SUCCESS",
  REGISTER_FAIL: "stocqres/login/REGISTER_FAIL",
  GET_ACCESS_TOKEN: "stocqres/auth/GET_ACCESS_TOKEN",
  GET_ACCESS_TOKEN_SUCCESS: "stocqres/auth/GET_ACCESS_TOKEN_SUCCESS",
  GET_ACCESS_TOKEN_FAIL: "stocqres/auth/GET_ACCESS_TOKEN_FAIL"
};

const initialState = {
  success: false,
  loading: false,
  accessToken: "",
  refreshToken: ""
};

export default function authenticationReducer(
  state = { initialState },
  action
) {
  switch (action.type) {
    case types.LOGIN:
      return { ...state, loading: true };
    case types.LOGIN_SUCCESS:
      return {
        ...state,
        loading: false,
        success: true,
        accessToken: action.data.tokens.accessToken,
        refreshToken: action.data.tokens.refreshToken,
        investorId: action.data.investorId,
        walletId: action.data.walletId
      };
    case types.LOGIN_FAIL:
      return { ...state, loading: false, success: false };
    case types.REGISTER:
      return { ...state, loading: true };
    case types.REGISTER_SUCCESS:
      return {
        state: [...state, action.response],
        loading: false,
        success: true
      };
    case types.REGISTER_FAIL:
      return { ...state, loading: false, success: false };
    case types.GET_ACCESS_TOKEN:
      return { ...state, loading: true };
    case types.GET_ACCESS_TOKEN_SUCCESS:
      return {
        state: [...state, action.response],
        loading: false,
        success: true,
        accessToken: action.accessToken
      };
    case types.GET_ACCESS_TOKEN_FAIL:
      return { ...state, loading: false, success: false };
    default:
      return state;
  }
}

export function login(command) {
  return (dispatch) => {
    dispatch(request());
    httpClient.post("/tokens/create", command).then(
      res => {
        var decodedToken = jwt_decode(res.data.accessToken)
        const {investorId, walletId} = decodedToken;
        AsyncStorage.setItem(constants.ACCESS_TOKEN, res.data.accessToken);
        AsyncStorage.setItem(constants.REFRESH_TOKEN, res.data.refreshToken);
        var sandbox = {
          tokens: res.data,
          investorId,
          walletId
        }
        dispatch(success(sandbox));
        goToHome();
      },
      error => {
        dispatch(failure(error));
      }
    );
  };
  function request() {
    return { type: types.LOGIN };
  }
  function success(data) {
    return { type: types.LOGIN_SUCCESS, data };
  }
  function failure(error) {
    return { type: types.LOGIN_FAIL, error };
  }
}

export function register(command) {
  return (dispatch) => {
    dispatch(request());
    httpClient.post("/investors", command).then(
      res => {
        dispatch(success(res.payload.data));
      },
      error => {
        dispatch(failure(error));
      }
    );
  };

  function request() {
    return { type: types.REGISTER };
  }
  function success(data) {
    return { type: types.REGISTER_SUCCESS, data };
  }
  function failure(error) {
    return { type: types.REGISTER_FAIL, error };
  }
}

export function getAccessToken() {
  return (dispatch, getState) => {
    var refreshToken = getState().authentication.refreshToken;
    dispatch(request());
    httpClient.get(`"refresh-tokens/${refreshToken}/refresh`).then(
      res => {
        AsyncStorage.setItem("AccessToken", res.payload.data);
        dispatch(success(res.payload.data));
      },
      error => {
        dispatch(failure(error));
      }
    );
  };

  function request() {
    return { type: types.GET_ACCESS_TOKEN };
  }
  function success(accessToken) {
    return { type: types.GET_ACCESS_TOKEN_SUCCESS, accessToken };
  }
  function failure(error) {
    return { type: types.GET_ACCESS_TOKEN_FAIL, error };
  }
}