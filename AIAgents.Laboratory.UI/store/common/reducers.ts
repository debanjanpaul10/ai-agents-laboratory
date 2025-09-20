import {
	TOGGLE_AGENT_TEST_DRAWER,
	TOGGLE_AGENTS_LIST_DRAWER,
	TOGGLE_EDIT_AGENT_DRAWER,
	TOGGLE_MAIN_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
} from "./actionTypes";

const initialState: any = {
	isNewAgentDrawerOpen: false,
	isAgentsListDrawerOpen: false,
	isEditAgentDrawerOpen: false,
	isTestAgentDrawerOpen: false,
	isLoading: false,
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

		default: {
			return {
				...state,
			};
		}
	}
}
