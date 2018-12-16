import axios from "axios";

export const types = {
  LOGIN: "stocqres/login/LOGIN",
  LOGIN_SUCCESS: "stocqres/login/LOGIN_SUCCESS",
  LOGIN_FAIL: "stocqres/login/LOGIN_FAIL",
  REGISTER: "stocqres/login/REGISTER",
  REGISTER_SUCCESS: "stocqres/login/REGISTER_SUCCESS",
  REGISTER_FAIL: "stocqres/login/REGISTER_FAIL"
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
        accessToken: action.payload.data.accessToken,
        refreshToken: action.payload.data.refreshToken
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
    default:
      return state;
  }
}

export function login(command) {
  return {
    type: types.LOGIN,
    payload: {
      request: {
        url: "/tokens/create",
        method: "POST",
        data: command
      }
    }
  };
}

export function register(command) {
  return {
    type: types.REGISTER,
    payload: {
      request: {
        url: "/investors",
        method: "POST",
        data: command
      }
    }
  };
}
