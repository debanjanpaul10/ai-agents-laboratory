import { Action, Dispatch } from "redux";
import {
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	DOWNLOAD_KNOWLEDGEBASE_FILE,
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	TOGGLE_AGENT_CREATE_SPINNER,
	TOGGLE_AGENTS_LIST_LOADING as TOGGLE_AGENTS_LIST_LOADER,
	TOGGLE_CHAT_RESPONSE_SPINNER,
	TOGGLE_DOWNLOAD_FILE_SPINNER,
	TOGGLE_EDIT_AGENT_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
	UPDATE_AGENT_DATA,
} from "@store/agents/actionTypes";
import {
	CreateNewAgentApiAsync,
	DeleteExistingAgentApiAsync,
	DownloadKnowledgebaseFileApiAsync,
	GetAgentByIdApiAsync,
	GetAgentsApiAsync,
	UpdateExistingAgentApiAsync,
} from "@shared/api-service";
import { CreateAgentDTO } from "@models/request/create-agent-dto";
import { AgentDataDTO } from "@models/response/agent-data-dto";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ToggleMainLoader } from "@store/common/actions";
import { AgentsToasterConstants } from "@helpers/toaster-constants";
import { ToggleAssociateAgentsDrawer } from "@store/workspaces/actions";
import { DownloadFileDTO } from "@models/request/download-file.dto";

export function ToggleNewAgentDrawer(isOpen: boolean) {
	return {
		type: TOGGLE_NEW_AGENT_DRAWER,
		payload: isOpen,
	};
}

export function ToggleCreateAgentSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_AGENT_CREATE_SPINNER,
		payload: isLoading,
	};
}

export function ToggleChatResponseSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_CHAT_RESPONSE_SPINNER,
		payload: isLoading,
	};
}

export function ToggleEditAgentSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_EDIT_AGENT_SPINNER,
		payload: isLoading,
	};
}

export function ToggleAgentsListLoader(isLoading: boolean) {
	return {
		type: TOGGLE_AGENTS_LIST_LOADER,
		payload: isLoading,
	};
}

export function ToggleDownloadFileSpinner(isLoading: boolean) {
	return {
		type: TOGGLE_DOWNLOAD_FILE_SPINNER,
		payload: isLoading,
	};
}

export function GetAllAgentsDataAsync(
	isFromManageAgents: boolean = false,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			if (isFromManageAgents) {
				dispatch(ToggleAgentsListLoader(true));
				dispatch(ToggleAssociateAgentsDrawer(true));
			}
			dispatch(ToggleMainLoader(true));

			const response = await GetAgentsApiAsync();
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_ALL_AGENTS_DATA,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			if (isFromManageAgents) dispatch(ToggleAgentsListLoader(false));
			dispatch(ToggleMainLoader(false));
		}
	};
}

export function GetAgentDataByIdAsync(agentId: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetAgentByIdApiAsync(agentId);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: GET_AGENT_BY_ID,
					payload: response.responseData,
				});
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		}
	};
}

export function CreateNewAgentAsync(
	newAgentData: CreateAgentDTO | FormData,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleCreateAgentSpinner(true));
			const response = await CreateNewAgentApiAsync(
				newAgentData,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CREATE_NEW_AGENT,
					payload: response.responseData,
				});

				dispatch(ToggleNewAgentDrawer(false));
				dispatch(GetAllAgentsDataAsync() as any);
				ShowSuccessToaster(AgentsToasterConstants.CREATE_AGENT);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleCreateAgentSpinner(false));
		}
	};
}

export function UpdateExistingAgentDataAsync(
	existingAgentData: AgentDataDTO | FormData,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditAgentSpinner(true));
			const response = await UpdateExistingAgentApiAsync(
				existingAgentData,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: UPDATE_AGENT_DATA,
					payload: response.responseData,
				});
				dispatch(GetAllAgentsDataAsync() as any);
				ShowSuccessToaster(AgentsToasterConstants.UPDATE_AGENT);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleEditAgentSpinner(false));
		}
	};
}

export function DeleteExistingAgentDataAsync(
	agentId: string,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditAgentSpinner(true));
			const response = await DeleteExistingAgentApiAsync(
				agentId,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DELETE_AGENT_DATA,
					payload: response.responseData,
				});

				dispatch(GetAllAgentsDataAsync() as any);
				ShowSuccessToaster(AgentsToasterConstants.DELETE_AGENT);
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleEditAgentSpinner(false));
		}
	};
}

export function DownloadKnowledgebaseFileAsync(
	downloadFileData: DownloadFileDTO,
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleDownloadFileSpinner(true));
			const response = await DownloadKnowledgebaseFileApiAsync(
				downloadFileData,
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DOWNLOAD_KNOWLEDGEBASE_FILE,
					payload: response.responseData,
				});

				return response.responseData;
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleDownloadFileSpinner(false));
		}
	};
}
