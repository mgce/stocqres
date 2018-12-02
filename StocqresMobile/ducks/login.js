export const types = {
    LOGIN: 'stocqres/login/LOGIN',
    LOGIN_SUCCESS:'stocqres/login/LOGIN_SUCCESS',
    LOGIN_FAIL: 'stocqres/login/LOGIN_FAIL'
}

const initialState  ={
    success: false
}

export default function loginReducer(state={initialState}, action){
    switch (action.type){
        case types.LOGIN:
            return {...state, loading: true}
        case types.LOGIN_SUCCESS:
            return {state:[...state, action.response], loading: false, success: true}
        case types.LOGIN_FAIL:
            return {...state, loading: false, success: false}
        default:
            return state;
    }
}

export function login(username, password){
    return{
        type: types.LOGIN,
        payload:{
            request:{
                url:'sign-in'
            }
        }
    }
}