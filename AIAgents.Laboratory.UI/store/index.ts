import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { configureStore } from "@reduxjs/toolkit";

import { AgentsReducer } from "./agents/reducers";
import { ChatReducer } from "./chat/reducers";
import { CommonReducer } from "./common/reducers";

export const store = configureStore({
	reducer: {
		AgentsReducer: AgentsReducer,
		ChatReducer: ChatReducer,
		CommonReducer: CommonReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
