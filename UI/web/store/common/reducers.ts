import {
	TOGGLE_DIRECT_CHAT_DRAWER,
	TOGGLE_DIRECT_CHAT_LOADER,
	TOGGLE_MAIN_SPINNER,
	GET_ALL_CONFIGURATIONS,
	GET_CONFIGURATION_BY_KEY_NAME,
	ADD_NEW_BUG_REPORT,
	TOGGLE_FEEDBACK_DRAWER,
	TOGGLE_FEEDBACK_LOADER,
	ADD_NEW_FEATURE_REQUEST,
	GET_TOP_ACTIVE_AGENTS,
} from "@store/common/actionTypes";

const initialState: any = {
	isLoading: false,
	isDirectChatOpen: false,
	isDirectChatLoading: false,
	configurations: {},
	configurationValue: {},
	newBugReport: {},
	featureRequest: {},
	isFeedbackDrawerOpen: false,
	isFeedbackDrawerLoading: false,
	topActiveAgents: [],
};

export function CommonReducer(state = initialState, action: any) {
	switch (action.type) {
		case TOGGLE_MAIN_SPINNER: {
			return {
				...state,
				isLoading: action.payload,
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
		case ADD_NEW_BUG_REPORT: {
			return {
				...state,
				newBugReport: action.payload,
			};
		}
		case ADD_NEW_FEATURE_REQUEST: {
			return {
				...state,
				featureRequest: action.payload,
			};
		}
		case TOGGLE_FEEDBACK_DRAWER: {
			return {
				...state,
				isFeedbackDrawerOpen: action.payload,
			};
		}
		case TOGGLE_FEEDBACK_LOADER: {
			return {
				...state,
				isFeedbackDrawerLoading: action.payload,
			};
		}
		case GET_TOP_ACTIVE_AGENTS: {
			return {
				...state,
				topActiveAgents: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
