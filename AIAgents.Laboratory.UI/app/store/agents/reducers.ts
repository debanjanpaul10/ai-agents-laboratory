import {
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	TOGGLE_SPINNER,
} from "./actionTypes";

const initialState: any = {
	agentsListData: [],
	agentData: {},
	isLoading: false,
};

export const AgentsReducer = (state = initialState, action: any) => {
	switch (action.type) {
		case GET_ALL_AGENTS_DATA: {
			return {
				...state,
				agentsListData: action.payload,
			};
		}
		case GET_AGENT_BY_ID: {
			return {
				...state,
				agentData: action.payload,
			};
		}
		case TOGGLE_SPINNER: {
			return {
				...state,
				isLoading: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
};
