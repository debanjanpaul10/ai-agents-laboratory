import {
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	UPDATE_AGENT_DATA,
	TOGGLE_AGENT_CREATE_SPINNER,
} from "./actionTypes";

const initialState: any = {
	agentsListData: [],
	agentData: {},
	isAgentCreateSpinnerLoading: false,
	agentCreatedResponse: false,
	updateAgentResponse: {},
	deleteAgentResponse: false,
};

export function AgentsReducer(state = initialState, action: any) {
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
		case CREATE_NEW_AGENT: {
			return {
				...state,
				agentCreatedResponse: action.payload,
			};
		}
		case UPDATE_AGENT_DATA: {
			return {
				...state,
				updateAgentResponse: action.payload,
			};
		}
		case DELETE_AGENT_DATA: {
			return {
				...state,
				deleteAgentResponse: action.payload,
			};
		}
		case TOGGLE_AGENT_CREATE_SPINNER: {
			return {
				...state,
				isAgentCreateSpinnerLoading: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
