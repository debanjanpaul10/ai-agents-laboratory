import {
	GET_ALL_WORKSPACES,
	GET_WORKSPACE_BY_ID,
	GET_WORKSPACE_GROUP_CHAT_RESPONSE,
	TOGGLE_ASSOCIATE_AGENTS_DRAWER,
	TOGGLE_CREATE_WORKSPACE_DRAWER,
	TOGGLE_CREATE_WORKSPACE_LOADER,
	TOGGLE_EDIT_WORKSPACES_LOADER,
	TOGGLE_WORKSPACES_LOADER,
} from "@store/workspaces/actionTypes";

const initialState: any = {
	isWorkspacesLoading: false,
	allWorkspaces: [],
	workspaceData: {},
	isEditWorkspaceLoading: false,
	isAddWorkspaceLoading: false,
	isAddWorkspaceDrawerOpen: false,
	isAssociateAgentsDrawerOpen: false,
	workspaceGroupChatResponse: {},
};

export function WorkspacesReducer(state = initialState, action: any) {
	switch (action.type) {
		case TOGGLE_WORKSPACES_LOADER: {
			return {
				...state,
				isWorkspacesLoading: action.payload,
			};
		}
		case TOGGLE_EDIT_WORKSPACES_LOADER: {
			return {
				...state,
				isEditWorkspaceLoading: action.payload,
			};
		}
		case TOGGLE_CREATE_WORKSPACE_LOADER: {
			return {
				...state,
				isAddWorkspaceLoading: action.payload,
			};
		}
		case TOGGLE_CREATE_WORKSPACE_DRAWER: {
			return {
				...state,
				isAddWorkspaceDrawerOpen: action.payload,
			};
		}
		case TOGGLE_ASSOCIATE_AGENTS_DRAWER: {
			return {
				...state,
				isAssociateAgentsDrawerOpen: action.payload,
			};
		}
		case GET_ALL_WORKSPACES: {
			return {
				...state,
				allWorkspaces: action.payload,
			};
		}
		case GET_WORKSPACE_BY_ID: {
			return {
				...state,
				workspaceData: action.payload,
			};
		}
		case GET_WORKSPACE_GROUP_CHAT_RESPONSE: {
			return {
				...state,
				workspaceGroupChatResponse: action.payload,
			};
		}

		default: {
			return {
				...state,
			};
		}
	}
}
