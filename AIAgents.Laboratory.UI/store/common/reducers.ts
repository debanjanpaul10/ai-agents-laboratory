import { TOGGLE_MAIN_SPINNER, TOGGLE_NEW_AGENT_DRAWER } from "./actionTypes";

const initialState: any = {
	isNewAgentDrawerOpen: false,
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

		default: {
			return {
				...state,
			};
		}
	}
}
