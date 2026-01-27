import {
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	UPDATE_AGENT_DATA,
	TOGGLE_CHAT_RESPONSE_SPINNER,
	TOGGLE_AGENT_CREATE_SPINNER,
	TOGGLE_EDIT_AGENT_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
	TOGGLE_AGENTS_LIST_LOADING,
	DOWNLOAD_KNOWLEDGEBASE_FILE,
	TOGGLE_DOWNLOAD_FILE_SPINNER,
} from "@store/agents/actionTypes";

const initialState: any = {
	agentsListData: [],
	agentData: {},
	isAgentCreateSpinnerLoading: false,
	agentCreatedResponse: false,
	updateAgentResponse: {},
	deleteAgentResponse: false,
	isChatDataLoading: false,
	isEditAgentDataLoading: false,
	isNewAgentDrawerOpen: false,
	isAgentsListLoading: false,
	downloadedFiles: {},
	isDownloadFileSpinnerLoading: false,
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
		case DOWNLOAD_KNOWLEDGEBASE_FILE: {
			return {
				...state,
				downloadedFiles: action.payload,
			};
		}
		case TOGGLE_AGENT_CREATE_SPINNER: {
			return {
				...state,
				isAgentCreateSpinnerLoading: action.payload,
			};
		}
		case TOGGLE_DOWNLOAD_FILE_SPINNER: {
			return {
				...state,
				isDownloadFileSpinnerLoading: action.payload,
			};
		}
		case TOGGLE_CHAT_RESPONSE_SPINNER: {
			return {
				...state,
				isChatDataLoading: action.payload,
			};
		}
		case TOGGLE_EDIT_AGENT_SPINNER: {
			return {
				...state,
				isEditAgentDataLoading: action.payload,
			};
		}
		case TOGGLE_NEW_AGENT_DRAWER: {
			return {
				...state,
				isNewAgentDrawerOpen: action.payload,
			};
		}
		case TOGGLE_AGENTS_LIST_LOADING: {
			return {
				...state,
				isAgentsListLoading: action.payload,
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
}
