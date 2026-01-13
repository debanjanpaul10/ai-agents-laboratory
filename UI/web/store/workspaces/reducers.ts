import {
	GET_ALL_WORKSPACES,
	TOGGLE_WORKSPACES_LOADER,
} from "@store/workspaces/actionTypes";

const initialState: any = {
	isWorkspacesLoading: false,
	allWorkspaces: [],
};

export function WorkspacesReducer(state = initialState, action: any) {
	switch (action.type) {
		case TOGGLE_WORKSPACES_LOADER: {
			return {
				...state,
				isWorkspacesLoading: action.payload,
			};
		}
		case GET_ALL_WORKSPACES: {
			return {
				...state,
				allWorkspaces: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
