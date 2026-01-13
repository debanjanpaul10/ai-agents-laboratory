import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";
import { configureStore } from "@reduxjs/toolkit";

import { AgentsReducer } from "@store/agents/reducers";
import { CommonReducer } from "@store/common/reducers";
import { ChatReducer } from "@store/chat/reducers";
import { ToolSkillsReducer } from "@store/tools-skills/reducers";

export const store = configureStore({
	reducer: {
		AgentsReducer: AgentsReducer,
		CommonReducer: CommonReducer,
		ChatReducer: ChatReducer,
		ToolSkillsReducer: ToolSkillsReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch: () => AppDispatch = useDispatch;
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;
