import {
	TOGGLE_AGENT_TEST_DRAWER,
	TOGGLE_AGENTS_LIST_DRAWER,
	TOGGLE_DIRECT_CHAT_DRAWER,
	TOGGLE_DIRECT_CHAT_LOADER,
	TOGGLE_EDIT_AGENT_DRAWER,
	TOGGLE_MAIN_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
	GET_ALL_CONFIGURATIONS,
	GET_CONFIGURATION_BY_KEY_NAME,
} from "@store/common/actionTypes";

const initialState: any = {
	isNewAgentDrawerOpen: false,
	isAgentsListDrawerOpen: false,
	isEditAgentDrawerOpen: false,
	isTestAgentDrawerOpen: false,
	isLoading: false,
	isDirectChatOpen: false,
	isDirectChatLoading: false,
	configurations: {},
	configurationValue: {},
};

export function CommonReducer(state = initialState, action: any) {
	switch (action.type) {
		case TOGGLE_NEW_AGENT_DRAWER: {
			return {
				...state,
				isNewAgentDrawerOpen: action.payload,
			};
		}
		case TOGGLE_MAIN_SPINNER: {
			return {
				...state,
				isLoading: action.payload,
			};
		}
		case TOGGLE_AGENTS_LIST_DRAWER: {
			return {
				...state,
				isAgentsListDrawerOpen: action.payload,
			};
		}
		case TOGGLE_EDIT_AGENT_DRAWER: {
			return {
				...state,
				isEditAgentDrawerOpen: action.payload,
			};
		}
		case TOGGLE_AGENT_TEST_DRAWER: {
			return {
				...state,
				isTestAgentDrawerOpen: action.payload,
			};
		}
		case TOGGLE_DIRECT_CHAT_DRAWER: {
			return {
				...state,
				isDirectChatOpen: action.payload,
			};
		}
		case TOGGLE_DIRECT_CHAT_LOADER: {
			return {
				...state,
				isDirectChatLoading: action.payload,
			};
		}
		case GET_ALL_CONFIGURATIONS: {
			return {
				...state,
				configurations: action.payload,
			};
		}
		case GET_CONFIGURATION_BY_KEY_NAME: {
			return {
				...state,
				configurationValue: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
