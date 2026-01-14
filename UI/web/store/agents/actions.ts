import { Action, Dispatch } from "redux";
import {
	CREATE_NEW_AGENT,
	DELETE_AGENT_DATA,
	GET_AGENT_BY_ID,
	GET_ALL_AGENTS_DATA,
	TOGGLE_AGENT_CREATE_SPINNER,
	TOGGLE_CHAT_RESPONSE_SPINNER,
	TOGGLE_EDIT_AGENT_SPINNER,
	TOGGLE_NEW_AGENT_DRAWER,
	UPDATE_AGENT_DATA,
} from "@store/agents/actionTypes";
import {
	CreateNewAgentApiAsync,
	DeleteExistingAgentApiAsync,
	GetAgentByIdApiAsync,
	GetAgentsApiAsync,
	UpdateExistingAgentApiAsync,
} from "@shared/api-service";
import { CreateAgentDTO } from "@models/create-agent-dto";
import { AgentDataDTO } from "@models/agent-data-dto";
import { ShowErrorToaster, ShowSuccessToaster } from "@shared/toaster";
import { ToggleMainLoader } from "@store/common/actions";

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

export function GetAllAgentsDataAsync(accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await GetAgentsApiAsync(accessToken);
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
			dispatch(ToggleMainLoader(false));
		}
	};
}

export function GetAgentDataByIdAsync(agentId: string, accessToken: string) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			const response = await GetAgentByIdApiAsync(agentId, accessToken);
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
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleCreateAgentSpinner(true));
			const response = await CreateNewAgentApiAsync(
				newAgentData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: CREATE_NEW_AGENT,
					payload: response.responseData,
				});

				dispatch(ToggleNewAgentDrawer(false));
				dispatch(GetAllAgentsDataAsync(accessToken) as any);
				ShowSuccessToaster("Agent created successfully");
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
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleEditAgentSpinner(true));
			const response = await UpdateExistingAgentApiAsync(
				existingAgentData,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: UPDATE_AGENT_DATA,
					payload: response.responseData,
				});
				dispatch(GetAllAgentsDataAsync(accessToken) as any);
				ShowSuccessToaster("Agent updated successfully!");
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
	accessToken: string
) {
	return async (dispatch: Dispatch<Action>) => {
		try {
			dispatch(ToggleMainLoader(true));
			const response = await DeleteExistingAgentApiAsync(
				agentId,
				accessToken
			);
			if (response?.isSuccess && response?.responseData) {
				dispatch({
					type: DELETE_AGENT_DATA,
					payload: response.responseData,
				});

				dispatch(GetAllAgentsDataAsync(accessToken) as any);
				ShowSuccessToaster("Agent deleted successfully!");
			}
		} catch (error: any) {
			console.error(error);
			if (error.message) ShowErrorToaster(error.message);
		} finally {
			dispatch(ToggleMainLoader(false));
		}
	};
}
