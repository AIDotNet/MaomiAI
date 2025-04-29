import { createSlice, configureStore } from '@reduxjs/toolkit'
import rootReducer from './reducer';

// 创建 Redux store
const store = configureStore({
    reducer: rootReducer
})

export default store;
