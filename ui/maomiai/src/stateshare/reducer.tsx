import { INCREMENT, DECREMENT, SET_USERNAME, SET_TEAM_ID, SET_CURRENT_TEAM } from './actions';

// 初始化状态
const initialState = {
    count: 0,
    username: '',
    teamId: '',
    currentTeam: null,
};

// 创建 reducer
const rootReducer = (state = initialState, action: any) => {
    switch (action.type) {
        case INCREMENT:
            return {
                ...state,
                count: state.count + 1,
            };
        case DECREMENT:
            return {
                ...state,
                count: state.count - 1,
            };
        case SET_USERNAME:
            return {
                ...state,
                username: action.payload,
            };
        case SET_TEAM_ID:
            return {
                ...state,
                teamId: action.payload,
            };
        case SET_CURRENT_TEAM:
            return {
                ...state,
                currentTeam: action.payload,
            };
        default:
            return state;
    }
};

export default rootReducer;
