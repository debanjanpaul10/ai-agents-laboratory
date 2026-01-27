import { Action, Dispatch } from "redux";
import {
	ADD_NEW_WORKSPACE_DATA,
	GET_ALL_WORKSPACES,
	GET_WORKSPACE_BY_ID,
	TOGGLE_ASSOCIATE_AGENTS_DRAWER,
	TOGGLE_CREATE_WORKSPACE_DRAWER,
	TOGGLE_CREATE_WORKSPACE_LOADER,
	TOGGLE_EDIT_WORKSPACES_LOADER,
	TOGGLE_WORKSPACES_LOADER,
} from "@store/workspaces/actionTypes";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import {
	CreateNewWorkspaceApiAsync,
	DeleteExistingWorkspaceApiAsync,
	GetAllWorkspacesDataApiAsync,
	GetWorkspaceByWorkspaceIdApiAsync,
	GetWorkspaceGroupChatResponseApiAsync,
	UpdateExistingWorkspaceDataApiAsync,
} from "@shared/api-service";
import { AgentsWorkspaceDTO } from "@models/response/agents-workspace-dto";
import { WorkspaceToasterConstants } from "@helpers/toaster-constants";
import { WorkspaceAgentChatRequestDTO } from "@models/request/workspace-agent-chat-request.dto";
import { ToggleChatResponseSpinner } from "@store/agents/actions";
import { GET_CHAT_RESPONSE } from "@store/chat/actionTypes";

export function ToggleWorkspacesLoader(isLoading: boolean) {
	return {
		type: TOGGLE_WORKSPACES_LOADER,
		payload: isLoading,
	};
}

export function ToggleEditWorkspacesLoader(isLoading: boolean) {
	return {
		type: TOGGLE_EDIT_WORKSPACES_LOADER,
		payload: isLoading,
	};
}

export function ToggleCreateWorkspaceLoader(isLoading: boolean) {
	return {
		type: TOGGLE_CREATE_WORKSPACE_LOADER,
		payload: isLoading,
	};
}

export function ToggleCreateWorkspaceDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_CREATE_WORKSPACE_DRAWER,
		payload: isOpen,
	};
}

export function ToggleAssociateAgentsDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_ASSOCIATE_AGENTS_DRAWER,
		payload: isOpen,
	};
}

export function GetAllWorkspacesDataAsync() {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleWorkspacesLoader(true));
			const response = await GetAllWorkspacesDataApiAsync();
			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_ALL_WORKSPACES,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleWorkspacesLoader(false));
		}
	};
}

export function GetWorkspaceByWorkspaceIdAsync(
	workspaceId: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleWorkspacesLoader(true));
			const response = await GetWorkspaceByWorkspaceIdApiAsync(
				workspaceId,
			);

			if (response?.isSuccess && response?.responseData)
				dispatch({
					type: GET_WORKSPACE_BY_ID,
					payload: response.responseData,
				});
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleWorkspacesLoader(false));
		}
	};
}

export function CreateNewWorkspaceAsync(
	agentsWorkspaceData: AgentsWorkspaceDTO | FormData,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleCreateWorkspaceLoader(true));
			const response = await CreateNewWorkspaceApiAsync(
				agentsWorkspaceData,
			);

			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: ADD_NEW_WORKSPACE_DATA,
					payload: response.responseData,
				});

				dispatch(GetAllWorkspacesDataAsync() as any);
				ShowSuccessToaster(WorkspaceToasterConstants.CREATE_WORKSPACE);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleCreateWorkspaceLoader(false));
		}
	};
}

export function DeleteExistingWorkspaceAsync(
	workspaceGuidId: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleWorkspacesLoader(true));
			const response = await DeleteExistingWorkspaceApiAsync(
				workspaceGuidId,
			);

			if (response?.isSuccess && response?.responseData) {
				dispatch(GetAllWorkspacesDataAsync() as any);
				ShowSuccessToaster(WorkspaceToasterConstants.DELETE_WORKSPACE);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleWorkspacesLoader(false));
		}
	};
}

export function UpdateExistingWorkspaceDataAsync(
	agentsWorkspaceData: AgentsWorkspaceDTO,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditWorkspacesLoader(true));
			const response = await UpdateExistingWorkspaceDataApiAsync(
				agentsWorkspaceData,
			);

			if (response?.isSuccess && response?.responseData) {
				dispatch(GetAllWorkspacesDataAsync() as any);
				ShowSuccessToaster(WorkspaceToasterConstants.UPDATE_WORKSPACE);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleEditWorkspacesLoader(false));
		}
	};
}

export function GetWorkspaceGroupChatResponseAsync(
	chatRequestDto: WorkspaceAgentChatRequestDTO,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleChatResponseSpinner(true));
			const response = await GetWorkspaceGroupChatResponseApiAsync(
				chatRequestDto,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_CHAT_RESPONSE,
					payload: response.responseData,
				});

				return response.responseData as string;
			}

			return null;
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleChatResponseSpinner(false));
		}
	};
}
