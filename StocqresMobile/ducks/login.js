export const LOGIN = 'stocqres/login/LOGIN'
export const LOGIN_SUCCESS = 'stocqres/login/LOGIN_SUCCESS'
export const LOGIN_FAIL = 'stocqres/login/LOGIN_FAIL'

const initialState  ={
    success: false
}

export default function reducer(state={initialState}, action){
    switch (action.type){
        case LOGIN:
            return {...state, loading: true}
        case LOGIN_SUCCESS:
            return {...state, loading: false, success: true}
        case LOGIN_FAIL:
            return {...state, loading: false, success: false}
        default:
            return state;
    }
}